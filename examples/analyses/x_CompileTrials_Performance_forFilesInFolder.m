function[compiled_data, compiled_performance]=x_CompileTrials_Performance_forFilesInFolder(todaysdate, path_to_data)

%% INPUT %% 
% todaysdate = string (e.g. '20231122')
% path_to_data= give path to data 
% e.g.path_to_data='/Volumes/human/dataset for methods paper/Trial Logging/New Scale/No Arms Version/SPS Task';
%%

cd(path_to_data)
% Get a list of CSV files in the current directory
delete '._SONA*.csv'
csv_files = dir('*.csv');

% Initialize an empty cell array to hold the combined data
compiled_data = {}; %[];
compiled_performance = cell(length(csv_files), 2); %zeros(size(SPS_csv_files,1),2); 

% Loop over the files
for iFiles = 1:length(csv_files)
    % Read the data from the current file
    data = readtable(csv_files(iFiles).name);
    % Split the filename into parts by "_"
    filename_parts = strsplit(csv_files(iFiles).name, '_');
    % Add a new column with the first part of the filename
    data.filename = repmat(filename_parts(1), height(data), 1);
    % Concatenate the data into the compiled_data cell array
    compiled_data = [compiled_data; table2cell(data)];

    
    correct_responses=sum(table2array(data(:,4)));
    trial_num=size(data(:,1),1);
    %not sure if overall_accuracy should be out of 1 or 100
    overall_accuracy=(correct_responses)/trial_num*100; %need to match this with MST output
    %overall_accuracy=(correct_responses)/trial_num*100;
    % Split the filename into parts by "_"
    filename_parts = strsplit(csv_files(iFiles).name, '_');
    % Assume the subject name is the first part of the filename
    subject_name = filename_parts{1};
    % Store the results
    compiled_performance{iFiles, 1} = subject_name;
    compiled_performance{iFiles, 2} = overall_accuracy;
end

% Convert the accuracy column to a numeric array
accuracy_array = cell2mat(compiled_performance(:, 2));

% Calculate the mean and standard deviation of the accuracies
mean_accuracy = mean(accuracy_array, 'omitnan');
std_accuracy = std(accuracy_array, 'omitnan');

% Calculate the z-score for each subject's accuracy
for iFiles = 1:length(csv_files)
    if isnan(compiled_performance{iFiles, 2})
        % If the accuracy is NaN, skip the z-score calculation or assign a default value
        compiled_performance{iFiles, 3} = NaN;  % or some default value
    else
        compiled_performance{iFiles, 3} = (compiled_performance{iFiles, 2} - mean_accuracy) / std_accuracy;
    end
end

% Save the combined data into a .mat file
save_filename=['compiled_trials_performance_' todaysdate];
save(save_filename, 'compiled_data','compiled_performance');

