﻿using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;

var aDotNetClient = new ADotNetClient();

var githubPipeline = new GithubPipeline
{
    Name = ".Net",

    OnEvents = new Events
    {
        Push = new PushEvent
        {
            Branches = new string[] { "main" }
        },

        PullRequest = new PullRequestEvent
        {
            Branches = new string[] { "main" }
        }
    },

    Jobs = new Jobs
    {
        Build = new BuildJob
        {
            RunsOn = BuildMachines.Windows2022,

            Steps = new List<GithubTask>
            {
                new CheckoutTaskV2
                {
                    Name = "Check Out"
                },

                new SetupDotNetTaskV1
                {
                    Name = "Setup Dot Net Version",

                    TargetDotNetVersion = new TargetDotNetVersion
                    {
                        DotNetVersion = "6.0.100-rc.2.21505.57",
                        IncludePrerelease = true
                    }
                },

                new RestoreTask
                {
                    Name = "Restore"
                },

                new DotNetBuildTask
                {
                    Name = "Build"
                },

                new TestTask
                {
                    Name = "Test"
                }
            }
        }
    }
};

aDotNetClient.SerializeAndWriteToFile(githubPipeline, "../../../../.github/workflows/dotnet.yml");