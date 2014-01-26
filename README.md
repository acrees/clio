Clio
====

A command-line tool for managing Visual Studio project files.

Usage
-----

`clio <project-directory> <operation> <arguments> <options>`

Operations:

 * `list` - prints a list of the files in the top level directory. Options:
  * `-f` full listing.
  * `-p <path>` start at this path.

 * `contains <file>` - whether or not a file is included in the project.

 * `add <action> <file>` - adds a file to the project.

*Caveat lector - very much a work in progress!*
