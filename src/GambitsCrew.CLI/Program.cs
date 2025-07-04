using CommandLine;
using GambitsCrew.CLI.Init;
using GambitsCrew.CLI.NewCommand;
using GambitsCrew.CLI.NewCondition;
using GambitsCrew.CLI.NewCrew;
using GambitsCrew.CLI.NewDeployment;
using GambitsCrew.CLI.NewGambit;
using GambitsCrew.CLI.NewOperator;
using GambitsCrew.CLI.NewSelector;
using GambitsCrew.CLI.Run;

return await Parser.Default.ParseArguments<
        InitOptions,
        NewCommandOptions,
        NewConditionOptions,
        NewCrewOptions,
        NewDeploymentOptions,
        NewGambitOptions,
        NewOperatorOptions,
        NewSelectorOptions,
        RunOptions
    >(args:args)
    .MapResult<
        InitOptions,
        NewCommandOptions,
        NewConditionOptions,
        NewCrewOptions,
        NewDeploymentOptions,
        NewGambitOptions,
        NewOperatorOptions,
        NewSelectorOptions,
        RunOptions,
        Task<int>
    >(
        InitCmd.RunAsync,
        NewCommandCmd.RunAsync,
        NewConditionCmd.RunAsync,
        NewCrewCmd.RunAsync,
        NewDeploymentCmd.RunAsync,
        NewGambitCmd.RunAsync,
        NewOperatorCmd.RunAsync,
        NewSelectorCmd.RunAsync,
        RunCmd.RunAsync,
        errs => Task.FromResult(1)
    );
