# Google Drive Time Machine
Google Drive Time Machine can help you restore your Google Drive back to how it used to be, in the good old times. Mainly designed to combat ransomware in the Google Drive world.

## Instructions

To set up the software, you will need to:
1. follow Step 1 from the Google Drive APIs Guide: https://developers.google.com/drive/v3/web/quickstart/dotnet
2. copy client_secret.json into the project (you can drag it into Visual Studio)
3. set the client_secret.json to copy to output directory always in the properties for the file

To use the software:
1. run the executable
1.a create a directory named cache in the same folder as the executable
2. a browser window will pop-up and ask you to authenticate to the API app that you created
3. select a date and time you wish to restore to and click the find button, this might take a while (check the console window for progress)
4. preview the recommended changes in the list and select the ones you want (ctrl or shift to multi-select)
5. click the restore button and files will be reverted one at a time, a copy of the data is saved in the cache folder with a json list of the filenames and ids
