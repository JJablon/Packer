# README #

This readme is a quick introduction to the discrete knapsack problem and it's practical implementation in C# using command-line to perform operations using 3rd party software. This app can be used for both simple backup and gathering unused data with a purpose to use in distant future, so unnecessarily taking up much HDD/SDD storage.

### Repo contains ###
* a program in C# that implements simple algorithm solving a knapsack problem by dividing a set of files to fit the DVD disc 

### History ###
I didn't happen to find any program to simply divide a group of files, significantly larger than a capacity of popular removable discs (CDs or DVDs), with possibility of accessing single file without the need of unpacking the whole group. That means, that e.g. option in win-rar with splitting into parts is in that case not suitable, as it needs to copy data from all of discs to a local drive, and then unpack the whole collections, even if trying to get a single file.

### How to use ###

The program uses command-line to instruct win-rar where files are located, and then creates independent archives with files.
Steps:
- user provides the path to files by a convenient windows forms GUI
- program analyses the files and divides it to fit the size of a disc best
- with a single click it generates a rar archive with user-set password and strong encryption


### Main features ###
* possibility of strong encryption of each part of files (provided by winrar)
* fully-automated behavior - just choose a directory and watch it divide into pieces!
* generating logs with lists of files - you always know where to find your file
* multithreading - depends on a system - program creates a number of simultaneous winrar processes, so the system can asssign them to multiple threads and cores, which is working fantastic in reality 

### How do I get set up? ###

* update Windows and necessary .NET libraries
* install 3rd party software (WinRar and Nero Suite) in proper directories 

### Known issues ###
The wide-range compatibility with various versions of 3rd party software is not yet provided (coming in next version). Also, automatically generating an iso file will be fixed in next version.
### Copyryghts ###
Using application is free, however, selling it or it's parts is strictly prohibited. Modifying it's code or it's fragments to use in any way should be discussed with the Author.