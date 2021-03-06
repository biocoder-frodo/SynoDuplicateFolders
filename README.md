SynoDuplicateFolders
=======

## Introduction
Built on top of SSH.NET, SynoDuplicateFolders is a Windows Forms client application for the Synology Storage Analyzer reports from DSM4.x and above.

## Features
* Uses SSH shell commands and SCP to copy zipped CSV reports to a local folder,
* Render the Duplicates report from the Storage Analyzer in a pair of treeview controls,
* Can automatically delete older analyzer.db files that can take up quite some space.
* Ability to use an external diff tool (like KDiff3) to evaluate the findings of the Synology Storage Analyzer
* Historical volume information in graphs

## Caveats
* I've developed this for a DS1512+ that is now patched to DSM 6.2.3-25423, with a English userinterface and the timezone set to GMT+01.
Since this application is parsing server output, this may not work for you, without making changes to code.

Tested with Storage Analyzer 2.0.1-0208.

![](https://github.com/biocoder-frodo/SynoDuplicateFolders/raw/master/wiki-images/synoreport-client.png)

[Goto the wiki](https://github.com/biocoder-frodo/SynoDuplicateFolders/wiki)
