Welcome to the readme!

-This parser is only valid for:
     -TMSS IRS .smhr files of versions: 10.1 <= version <= 10.6.
     -Database files (no extension), generated from software that produces .smhr files. 
     -.prm files. See later comments for parsing strategy.
     -.tbl files. See later comments for parsing strategy.
     -Folders containing the above files. If a folder is selected, it will parse all relevant files and automatically write output files to the 
         same directory given the name <filename>.csv.         


-To parse .smhr files, you first need to parse a database file (no extension). The unclassified example is MslDBList in Example_Files. 
     -This is because we would like to map the adp number to the system name. 
     -This creates an xml file under %LOCALAPPDATA%\MAFTL\ParserDatabaseFile which is read when performing the adp number to system name mapping.
          -You do not need to manually create this directory.
          -To access this folder, copy and paste "%LOCALAPPDATA%\MAFTL\ParserDatabaseFile" to the windows explorer. 
     -If writing to %LOCALAPPDATA% requires elevated permissions, please contact garrett.youmans@parsons.com. 
          -(For my reference: PostProcessDatabase@DatabaseData and GetADPNameFromSystemName@CombinedSmhrData)


-When entering any path to the console, be sure to enter the complete directory for the program to locate and write to the correct directory. 
     -It is recommended to specify output as .csv, as it is formatted as <Key>,<Value>.
     -When the database is built and the program is more mature, the long term goal is probably going to be to import the data directly from this parser to the 		database rather than write to a file, but in the interim, we are writing to a file.
 
-After discussion with Nathan Vardaman, the current strategy to parsing .prm and .tbl files is to read only from commented lines in the header. 
     -These commented lines are NOT there by default, and have to be manually entered. 
          -For .prm files, the parsable text is structured as the following, with spaces between objects on the same line after the comment symbol ('%'): 
			Line	|	Field
			1	|	Classification
			2	|	ADP_Name
			2	|	Object_Name
          -For .tbl files, also with spaces between objects on the same line after the comment symbol ('#'):
			Line	|	Field
			1	|	Classification
			2	|	ADP_Name
			2	|	Object_Name
			2	|	Frequency_Band

-Aside/FYI: the data dictionary is asking for items that are not present in the associated files. Eg: prm and waveband name.

If you have any questions or concerns about this program, please email garrett.youmans@parsons.com.