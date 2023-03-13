# ConditionTable Project

It is divided into two as project inquiry screen and rule adding screen.

Returns results based on predefined value ranges in the inquiry panel

## The values to be defined are added according to certain rules. These 

1. The value to be inserted must not be included in any previously inserted value range
2. Equality symbols are important when adding values
3. Plus infinity and minus infinity are covered according to the upper and lower bounds of the first value added.

## Impoartant Points 
* CSV file type is used as database
* The database will be automatically added to the temp folder on disk c.(If you don't have a temp folder, create it)
* If the file is desired to be deleted, the Truncate Table button can be used.
* The Result of the rules can be rearranged
* Autofac is used as a dependency resolver
* Used CSVHelper for CSV operations
* JS library is Jquery


### Database Path can be set according to desired file path in Appsettings
