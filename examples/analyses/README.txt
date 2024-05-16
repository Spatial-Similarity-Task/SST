#MATLAB Code Documentation 

###x_CompileTrials_Performance_forFilesInFolder.m**

**Inputs:**  
- todaysdate: Current date in 'YYYYMMDD' format.  
- path_to_data: Directory path of CSV files. 

**Description:** This function compiles and processes trial data from multiple CSV files, calculates overall accuracy and z-scores, and computes accuracy based on arm separation. The results are saved as a mat file with the following columns: trial_num, target_arm, lure_arm, subject_response, sample_phase_duration, sample_phase_choice_duration,  test_phase_duration, and test_phase_choice_duration.  

###x_plot_CompiledTrialsFiles.m 

**Input: ** 
- compiled_trials_file: Path to the compiled trials file. 

**Description:** This function loads the compiled trials file, calculates overall accuracy by arm separation, and plots average accuracy versus spatial separation. The resulting plot provides a visual representation of how participant performance varies with spatial separation. 

###x_plot_SubjectTrajectories.m 

**Input: ** 
- subjectID: Subject identifier. 
- folder: Directory path of CSV files. 

**Description:** This function visualizes a subject's trajectories during sample and test phases by extracting time, X and Z positions, and phase data from CSV files. It plots X and Z positions for each phase in separate subplots. The resulting plots provide a visual representation of the subject's movement during the sample and test phases across all trials. 

###x_createAschedulePLS.m 

**Inputs: ** 
- arm_separations: List of arm separations to use   
- total_trials: Total number of trials 

**Description:** This function generates two randomized schedules for the SST by creating and randomizing combinations of target and lure positions for each separation, then flipping columns in the second schedule. All combinations have been saved as allCombinations_forSchedule.mat. 
 
