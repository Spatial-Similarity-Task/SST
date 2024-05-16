function output = x_plot_SubjectTrajectories(subjectID, folder)

%% INPUT %%
%folder = Specify the folder where the CSV files are located 
% e.g. '/Volumes/human/dataset for methods paper/Trial Logging/New Scale/No Arms Version/SPS Task/SONA142_1_20240112_Trials';
%% 

% Get a list of all CSV files in the folder
files = dir(fullfile(folder, 'Trial_*.csv'));
% Loop over the files
for iFile = 1:length(files)
    % Construct the full file name and path
    file = fullfile(files(iFile).folder, files(iFile).name);

    % Read the data from the current file
    T = readtable(file);

    % Extract the necessary columns
    time = T.time;
    posX = T.posX;
    posZ = T.posZ;
    phase = T.phase;

    % Separate the data for the sample phase and the test phase
    sample_indices = phase == "Sample";
    test_indices = phase == "Test";

    posX_sample = posX(sample_indices);
    posZ_sample = posZ(sample_indices);

    posX_test = posX(test_indices);
    posZ_test = posZ(test_indices);

    % Plot the subject's trajectory in the sample phase
    subplot(1,2,1)
    plot(posX_sample, posZ_sample);
    hold on;

    % Plot the subject's trajectory in the test phase
    subplot(1,2,2)
    plot(posX_test, posZ_test);
    hold on;
end

% Add titles and labels to the sample phase plot
subplot(1,2,1)
title(['Subject ' subjectID ' Trajectory in Sample Phase']);
xlabel('Position X');
ylabel('Position Z');
hold off;

% Add titles and labels to the test phase plot
subplot(1,2,2)
title(['Subject ' subjectID ' Trajectory in Test Phase']);
xlabel('Position X');
ylabel('Position Z');
hold off;