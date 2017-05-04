SynoDuplicateFolders
=======

## Introduction
Built on top of SSH.NET, SynoDuplicateFolders is a Windows Forms client application for the Synology Storage Analyzer reports from DSM4.x and above.

## Features
* Uses SSH shell commands and SCP to copy zipped CSV reports to a local folder,
* Render the Duplicates report from the Storage Analyzer in a pair of treeview controls,
* Can automatically delete older analyzer.db files that can take up quite some space.

## Caveats
* I've developed this for a DS1512+ that is now patched to DSM 6.1, with a English userinterface and the timezone set to GMT+01.
Since this application is parsing server output, this may not work for you, without making changes to code.

Tested with Storage Analyzer 2.0.0-0158.

![](https://github.com/biocoder-frodo/SynoDuplicateFolders/raw/master/wiki-images/synoreport-client.png)