using LibGit2Sharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GitReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var pathToRepository = @"F:\GitHub\suvoda\suvoda-services.IRT\";
            var studyList = new List<StudyInfo>(); 

            using (var repo = new Repository(pathToRepository))
            {
                var remoteBranches = repo.Branches.Where(x => x.IsRemote).ToList();

                remoteBranches.ForEach(branch =>
                {

                    Commands.Checkout(repo, branch);

                    var study = new StudyInfo
                    {
                        StudyName = branch.FriendlyName
                    };

                    Console.WriteLine(study.StudyName);

                    var modules = new List<string>();
                    var regex = new Regex(".*suvoda[.]irt[.]modules.*[.]dll", RegexOptions.IgnoreCase | RegexOptions.Singleline);


                    repo.Index.ToList().Where(x => regex.IsMatch(x.Path)).ToList().ForEach(x =>
                    {
                        var moduleName = GetModuleName(x.Path);

                        if (!modules.Contains(moduleName))
                        {
                            modules.Add(moduleName);
                        }
                    });

                    modules.ForEach(x => Console.WriteLine("\t" + x));

                    study.Modules = modules;
                    studyList.Add(study);


                });

                File.WriteAllText("studyInfo.json", JsonConvert.SerializeObject(studyList,Formatting.Indented));
            }
        }

        static string GetModuleName(string path)
        {
            return path.Split('/').Last().Replace(".dll", "").Replace("Suvoda.IRT.Modules.", "");
        }
    }
}
