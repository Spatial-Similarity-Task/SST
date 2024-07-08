# SST

## Overview 
The SST is a behavioral task designed to tax pattern separation in the visuospatial domain.  

 The task suite consists of different modules in addition to the task. These are baseline, training, and practice, all of which should be run with participants before the task. Also included are Training 2 which is a non-verbal version of the training module, and SST 2 which is an alternate version of the SST task including wall partitions. 

## Dependencies 
SST is written in C#, and do not require that Unity be install on your computer to run.

## Install 
You can clone the repository or download the SST.zip file [here](https://ucsdcloud-my.sharepoint.com/:u:/g/personal/mborzell_ucsd_edu/EY_sue4yOuxNkvU-cZ9fKNMBpe5Apq9iGfa-1pZhIOZ9iQ?e=NFnai7).


## Reference
If you use this task and/or incorporate this code into your project, kindly cite:
citation TBD
Direct link: TBD

## Data Output 
The output of SST are an overall CSV file for the session (A) and individual CSV files for each trial of the session (B). 
![figures_20240509-05](https://github.com/Spatial-Similarity-Task/SST/assets/169395756/592d9cf8-962a-42bc-9eaf-0ce30ad24928)

Columns in A include: 
- **Trial_num**: Trial number
- **Target_arm, lure_arm**: Arm combination for each trial- target and lure arms
- **Subject_response**: Subject response for each trial (1=correct, 0=incorrect)
- **sample_phase_duration**: Time from when start box door opens to when subject returns to start box in sample phase
- **sample_phase_choice_duration**: Time from when start door opens to when subject makes choice in sample phase; sample phase reaction time
- **test_phase_duration**: Time from when start door opens to when subject returns to start box in test phase
- **test_phase_choice_duration**: Time from when start door opens to when subject makes choice in test phase; test phase reaction time

Columns in B include: 
- **time**: Time in seconds
- **posX**: The subject's movement along the x-axis
- **posY**: The subject's movement along the y-axis
- **event**: Event in task (e.g. Whether prompt is showing or if subject is performing the task)
- **phase**: The phase of a trial (Sample, Test)
- **Input**: Logs Xbox controller inputs and button presses 
