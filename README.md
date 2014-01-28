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

 * `find <path>` - finds a file or folder in the project. Options
  * `-r` (recursive) show full tree of all descendants.
  * `-c` (children) show a flat list of the immediate children only.

 * `add <action> <file>` - adds a file to the project.

*Caveat lector - very much a work in progress!*
