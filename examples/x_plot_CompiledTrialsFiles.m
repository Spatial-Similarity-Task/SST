function output= x_plot_CompiledTrialsFiles(compiled_trials_file)

%% INPUT %%
% compiled_trials_file= the output file from
% x_CompileTrials_Performance_forFilesInFolder.m
% e.g. compiled_trials_performance_20240506.mat
%%

load(compiled_trials_file);

correct_responses1=sum(cell2mat(compiled_data(:,4)));
trial_num1=size(compiled_data(:,1),1);
overall_acccuracy1=(correct_responses1/trial_num1)*100;
target_arm1=cell2mat(compiled_data(:,2));
lure_arm1=cell2mat(compiled_data(:,3));
arm_separation1=abs(target_arm1-lure_arm1)-1;
what_are_seps1=unique(arm_separation1);

for iSep=1:size(what_are_seps1,1)
    idx_arm_sep1=find(arm_separation1==what_are_seps1(iSep));
    mini_table_for_that_sep=compiled_data(idx_arm_sep1,:);
    mini_matrix_for_that_sep=compiled_data(idx_arm_sep1,:);

    %computations below are saved into arm_separation_performance matrix 
    num_trial_for_sep=size(mini_matrix_for_that_sep(:,4),1)

    arm_separation_performance1(1,iSep)=sum(cell2mat(mini_matrix_for_that_sep(:,4)))/num_trial_for_sep;
    arm_separation_performance1(2,iSep)=mean(cell2mat(mini_matrix_for_that_sep(:,5)));
    arm_separation_performance1(3,iSep)=mean(cell2mat(mini_matrix_for_that_sep(:,6)));
    arm_separation_performance1(4,iSep)=mean(cell2mat(mini_matrix_for_that_sep(:,7)));
    arm_separation_performance1(5,iSep)=mean(cell2mat(mini_matrix_for_that_sep(:,8)));
    
    %standard error is calculated by dividing the standard deviation by the sample size's square root.
    %calculate with the std of the correct response for that sep
    %(mini_matrix_for_that_sep(:,4)) divided by the sqrt of the number of
    %trials total for that sep 
    arm_separation_performance1(6,iSep)=std(cell2mat(mini_matrix_for_that_sep(:,4)))/sqrt(length(mini_matrix_for_that_sep(:,4))); %standard error 
end 

subject_performance_summary1=[what_are_seps1'; arm_separation_performance1];
%subject_performance_summary=[what_are_seps'; arm_separation_performance(:,1:2:5)]
how_many_separations=size(subject_performance_summary1,2);

plot(1:how_many_separations, subject_performance_summary1(2,:), 'ro')
hold on; line(1:how_many_separations, subject_performance_summary1(2,:))
title('Average-Accuracy as a function of spatial separation-rescaled/rat-sized maze, no arms')
%title('accuracy as a function of target-foil separation')
xlabel('spatial separation distance')
ylabel('mean percentage of correct')
xticklabels({0,1,2,3,4,5,6, 17,21})
ylim([0 1])
