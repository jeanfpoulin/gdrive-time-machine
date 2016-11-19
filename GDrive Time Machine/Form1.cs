using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Web.Script.Serialization;

namespace GDrive_Time_Machine
{
    public partial class Form1 : Form
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json
        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "GDrive Time Machine";
        static string downloadPath;

        static DriveService service;

        static DateTime dateModifiedSince_value = DateTime.Now;

        public class RestoreFileItem
        {
            public string fileId;

            public string current_name;
            public DateTime current_modifiedDate;
            public string current_mimeType;

            public string previous_revisionId;
            public string previous_name;
            public DateTime previous_modifiedDate;

            public string toString;

            public string Name
            {
                get
                {
                    return toString;
                }
            }

            public string Value
            {

                get
                {
                    return fileId;
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
            initializeGDrive();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dateModifiedSince_ValueChanged(object sender, EventArgs e)
        {
            dateModifiedSince_value = dateModifiedSince.Value.Date + timeModifiedSince.Value.TimeOfDay;
            Console.WriteLine(dateModifiedSince_value.ToString());
        }

        private void timeModifiedSince_ValueChanged(object sender, EventArgs e)
        {
            dateModifiedSince_value = dateModifiedSince.Value.Date + timeModifiedSince.Value.TimeOfDay;
            Console.WriteLine(dateModifiedSince_value.ToString());
        }


        private void btnFindRevisions_Click(object sender, EventArgs e)
        {

            List < RestoreFileItem > results = getTargetFiles();

            lstResults.DataSource = results;
            lstResults.DisplayMember = "Name";
            lstResults.ValueMember = "Value";


        }

        private List<RestoreFileItem> getTargetFiles()
        {
            List<RestoreFileItem> results = new List<RestoreFileItem>();

            /** CODE HERE **/

            //Find all files
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 1000; //TODO CHANGE TO VERY LARGE VALUE
            listRequest.Q = "name contains 'b4bc'"; //SEARCH FOR RANSOMWARE
            listRequest.Fields = "nextPageToken, files(id, name, modifiedTime, mimeType, size, lastModifyingUser(me))";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
            Console.WriteLine("Found " + files.Count + " files...");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.Size == null) continue;    // Only look at binary files
                    if (file.ModifiedTime < dateModifiedSince_value) continue; // Only look at files recently changed
                    //if (!file.LastModifyingUser.Me.Value) continue; // Only look at files modified by current user (RANSOMWARE)
                    if (!file.Name.Contains("b4bc")) continue; // Only look for RANSOMWARE pattern 1

                    //Create object for revisionresults
                    RestoreFileItem result = new RestoreFileItem();
                    result.fileId = file.Id;
                    result.current_modifiedDate = file.ModifiedTime.Value;
                    result.current_name = file.Name;
                    result.current_mimeType = file.MimeType;


                    //Check if file has previous revisions...
                    RevisionsResource.ListRequest revisionListRequest = service.Revisions.List(file.Id);
                    revisionListRequest.PageSize = 10;
                    revisionListRequest.Fields = "kind, revisions(id, modifiedTime, originalFilename)";

                    bool foundValueRevision = false;

                    try
                    {

                        IList<Google.Apis.Drive.v3.Data.Revision> revisions = revisionListRequest.Execute().Revisions;
                        if (revisions != null && revisions.Count > 1) //At least something to go back to other than current version
                        {
                            foreach (var revision in revisions)
                            {
                                if (revision.ModifiedTime < dateModifiedSince_value)
                                {
                                    foundValueRevision = true;
                                    Console.WriteLine("Found valid revision for file " + file.Name + "... revision from date " + revision.ModifiedTime.ToString());
                                    result.previous_revisionId = revision.Id;
                                    result.previous_modifiedDate = revision.ModifiedTime.Value;
                                    result.previous_name = revision.OriginalFilename;
                                }
                            }
                        }
                        else
                        {
                            //NO REVISIONS
                            continue;
                        }

                    }
                    catch (Exception e) { }


                    if(foundValueRevision)
                    {
                        //Good revision found
                        string toString = result.current_name + " (" + result.current_modifiedDate.ToString() + ") -> " + result.previous_name + " (" + result.previous_modifiedDate.ToString() + ")";
                        result.toString = toString;

                        results.Add(result);
                    }
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }

            /** END HERE **/

            //Save results to disk for sanity
            var json = new JavaScriptSerializer().Serialize(results);
            System.IO.File.AppendAllText(downloadPath + "\\restorable.lst", "[date]" + dateModifiedSince_value.ToString() + "[/date]" + json);

            return results;
        }



        private void btnRevertSelected_Click(object sender, EventArgs e)
        {
            foreach(Object item in lstResults.SelectedItems)
            {
                RestoreFileItem file = (RestoreFileItem)item;
                Console.WriteLine("Starting restore of... "+file.current_name);

                // Do restore
                string fileCachePath = downloadPath + "\\cache\\" + file.fileId + ".bin";

                Google.Apis.Drive.v3.Data.File f = service.Files.Get(file.fileId).Execute();

                //Download Revision
                MemoryStream stream = new MemoryStream();
                RevisionsResource.GetRequest dlRevision = new RevisionsResource.GetRequest(service, file.fileId, file.previous_revisionId);
                dlRevision.Download(stream);
                //service.Files.Get(revision.Id).Download(stream);
                System.IO.File.WriteAllBytes(fileCachePath, stream.ToArray());

                //Replace revision
                //System.IO.FileStream fs = new FileStream(downloadPath + "\\download\\"+ revision.OriginalFilename, FileMode.Open);
                FilesResource.UpdateMediaUpload fUpload = service.Files.Update(f, file.fileId, stream, file.current_mimeType);
                f.Name = file.previous_name;
                f.Id = null;
                fUpload.Fields = "name";
                var uploadStatus = fUpload.Upload();

                if (uploadStatus.Exception != null) MessageBox.Show("Error Uploading File!");
            }
        }

        private void initializeGDrive()
        {
            UserCredential credential;
            downloadPath = System.Environment.CurrentDirectory;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.CurrentDirectory;
                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }
        







        //ORIGINAL CODE FROM TESTING DO NOT EDIT
        private void consoleRun()
        {

            UserCredential credential;
            downloadPath = System.Environment.CurrentDirectory;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.CurrentDirectory;
                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 100;
            listRequest.Fields = "nextPageToken, files(id, name, modifiedTime, mimeType)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
            Console.WriteLine("Files:");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.Name != "Sample document RANSOM.txt") continue;
                    Console.WriteLine("{0} - {1} ({2})", file.ModifiedTime, file.Name, file.Id);
                    RevisionsResource.ListRequest revisionListRequest = service.Revisions.List(file.Id);
                    revisionListRequest.PageSize = 100;
                    revisionListRequest.Fields = "kind, revisions(id, modifiedTime, originalFilename, md5Checksum)";

                    IList<Google.Apis.Drive.v3.Data.Revision> revisions = revisionListRequest.Execute().Revisions;
                    if (revisions != null && revisions.Count > 0)
                    {
                        foreach (var revision in revisions)
                        {
                            Console.WriteLine("{0} - {1} ({2})", revision.ModifiedTime, revision.OriginalFilename, revision.Id);

                            //Replace original file
                            //Rename
                            Google.Apis.Drive.v3.Data.File f = service.Files.Get(file.Id).Execute();
                            /*FilesResource.UpdateRequest fUpdate = new FilesResource.UpdateRequest(service, f, file.Id);
                            fUpdate.*/

                            //Download Revision
                            MemoryStream stream = new MemoryStream();
                            RevisionsResource.GetRequest dlRevision = new RevisionsResource.GetRequest(service, file.Id, revision.Id);
                            dlRevision.Download(stream);
                            //service.Files.Get(revision.Id).Download(stream);

                            //Replace revision
                            //System.IO.FileStream fs = new FileStream(downloadPath + "\\download\\"+ revision.OriginalFilename, FileMode.Open);
                            FilesResource.UpdateMediaUpload fUpload = service.Files.Update(f, file.Id, stream, f.MimeType);
                            f.Name = revision.OriginalFilename;
                            f.Id = null;
                            fUpload.Fields = "name";
                            var uploadStatus = fUpload.Upload();

                            //Console.WriteLine(fUpload.ResponseBody.ToString());

                            //fs.Close();
                            break;

                            //Download Revision
                            /*System.IO.FileStream fs = new FileStream(downloadPath + "\\download\\"+ revision.OriginalFilename, FileMode.Create);
                            RevisionsResource.GetRequest dlRevision = new RevisionsResource.GetRequest(service, file.Id, revision.Id);
                            dlRevision.Download(fs);
                            service.Files.Get(revision.Id).Download(fs);
                            fs.Flush();
                            fs.Close();
                            break;*/
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }
            Console.Read();
        }
    }
}
