# README #

This readme is a quick introduction to the discrete knapsack problem and it's practical implementation in C# using only in-built .NET mechanisms. This app can be used for both simple backup and gathering unused data with a purpose to use in distant future, so unnecessarily taking up much HDD/SDD storage.

### Repo contains ###
* a program in C# that implements simple algorithm solving a knapsack problem by dividing a set of files to fit the DVD disc 

### History ###
I didn't happen to find any program to simply divide a group of files, significantly larger than a capacity of popular removable discs (CDs or DVDs), with possibility of accessing single file without the need of unpacking the whole group. That means, that e.g. option in win-rar with splitting into parts is in that case not suitable, as it needs to copy data from all of discs to a local drive, and then unpack the whole collections, even if trying to get a single file.

### Steps ###

- user provides the path to files by a convenient windows forms GUI
- program analyses the files and divides it to fit the size of a disc best
- with a single click it generates a gzip archive in desired location


### Main features ###

* fully-automated behavior - just choose a directory and watch it divide into pieces!
* generating logs with lists of files - you always know where to find your file
* multithreading - program creates a number of simultaneous background workers, so the system can assign them to multiple threads and cores, which works fantastic in reality 
* user-friendly Graphic Interface
* intuitive, simple setup 

### How do I get set up? ###

* update Windows and necessary .NET libraries
* note that the program is compiled using .NET 4.0
* please note that splitting large portions of files can result in significant performance boost (not less than 8GB for Intel i3, 16GB for i5,  32GB for i7 is required to use multithreading)

### Known issues ###
Please make an issue to let me know about incompatibilities or errors.

### Coming in next version ###
* possibility of strong encryption (AES 1024) of each part of files (provided by in-built .NET libraries)

### Credits ####
Microsoft MSDN resources:
* [https://msdn.microsoft.com/pl-pl/library/system.componentmodel.backgroundworker(v=vs.110).aspx](Link URL)
* [https://msdn.microsoft.com/pl-pl/library/system.io.compression.gzipstream(v=vs.110).aspx](Link URL)
### Copyryghts ###
Using application is free, however, selling it or its parts is strictly prohibited. Modifying its code or it's fragments to use in any way should be discussed with the Author.