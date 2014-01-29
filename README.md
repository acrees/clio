Clio
====

A command-line tool for managing Visual Studio project files.

Usage
-----

`clio <project-directory> <operation> <arguments> <options>`

Operations:

 * `list` - prints a list of the files in the top level directory. Options:
  * `-r` recursive (show full tree).
  * `-p <path>` start at this path.
  * Example: `clio src/Website list -rp Scripts/`

 * `find <path>` - finds a file or folder in the project. Options
  * `-r` show full tree of all descendants.
  * `-c` show a flat list of the immediate children only.
  * Example: `clio src/Website/ find Areas/Admin -r`

 * `add <action> <file>` - adds a file to the project.
 * Example: `clio src/Website/ add Compile Controllers/HomeController.cs`

*Caveat lector - very much a work in progress!*
