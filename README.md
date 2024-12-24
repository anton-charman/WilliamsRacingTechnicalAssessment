# WilliamsRacingTechnicalAssessment

Introduction

This document provides instructions for running the Visual Studio solution for my Williams Racing technical assessment submission and a brief design process background. 

The application was designed in C# and WPF using the Model-View-ViewModel (MVVM) design pattern to allow for extensibility, maintainability and separations of concerns between views and business logic.

Operating instructions

1) Setting the data folder location
The relevant data folder must be hard-coded by the user inside the application because BrowseFolderDialog in C# technically break MVVM - see Ref. [1]. Hence, go to:

WilliamsRacingTechnicalAssessment/ViewModels/MainWindowViewModel.cs

and paste your path to the required datasets in Line 14. See below. 

![Capture](https://github.com/user-attachments/assets/9ddd0f68-30b0-41f0-8cc2-f7b920811efd)

3) Using the application
   
Upon opening the application the user will be greeted with the following UI:

![image](https://github.com/user-attachments/assets/9271ad5c-a5fa-4ce2-903c-fe8e8771ebcc)

To navigate the application, the user can:

⦁	Switch between circuit and driver summaries via tab selection.

⦁	Search for fields which bring up matching circuits and drivers dynamically. 

⦁	Press the clear button to reset all filters. 

⦁	Select a circuit or diver in the list view to bring up a relevant summary.

Design process

The user requirements asked for the following properties from circuits.json and drivers.json:

	1) Circuits: fastest lap and num. total races
	2) Drivers: num. times on podium and num. races entered
 
Analysing the provided data structures in the provided files allowed me to spot the following data relations:

![image](https://github.com/user-attachments/assets/c798c7f8-c24e-4db7-8f29-ad9ce0b4114d)

Therefore, 

	1) Circuit summary needs races.csv and lap_times.csv 
	2) Driver summary needs driver_standings.csv

Finally, JSON serialise the circuit/driver data, extract and store the used properties above and LINQ query the resulting lists for the properties in the user requirements. 

Conclusion

Key results

The user can manipulate F1 circuit and driver standings data dynamically in the designed application. 

Future work

There exists a bug where if a circuit or driver object is selected in the relevant list view, when the UI search boxes are typed into the data isn't fully cleared in the top-right-hand summary panel.  

References

[1] https://stackoverflow.com/questions/61937241/browsefolderdialog-does-this-break-mvvm



