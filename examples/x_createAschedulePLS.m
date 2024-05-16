function [schedule1, schedule2] = createAschedulePLS(arm_separations, total_trials)

%% INPUT %%
%   arm_separations = [3, 6,17, 21];
%   total_trials = 40;
%%

% Initialize the output variables
    schedule1 = [];
    schedule2 = [];
    % Calculate the number of trials per separation
    num_trials = floor(total_trials / length(arm_separations));
    % Loop over each arm separation
    for sep = arm_separations
        target = 1:1:27;
        lure = target + sep + 1;
        combos = [target; lure]';
        threshold = find(combos(:,2) == 27);
        combos = combos(1:threshold,:);
        combos_b = flip(combos,2);
        combos = [combos_b; combos];

        % Select a random subset of the combos for this separation
        rand_indices = randperm(size(combos,1), num_trials);
        picks = combos(rand_indices,:);

        % Add the picks to the output variables
        schedule1 = [schedule1; picks];
        schedule2 = [schedule2; picks(:,[2 1])];  % Flip the columns for schedule2
    end

    % Shuffle the order of the picks
    schedule1 = schedule1(randperm(size(schedule1, 1)), :);
    schedule2 = schedule2(randperm(size(schedule2, 1)), :);
end
