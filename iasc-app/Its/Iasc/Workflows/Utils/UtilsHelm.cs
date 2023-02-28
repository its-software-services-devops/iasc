using System;
using System.Collections.Generic;
using Its.Iasc.Workflows.Models;

namespace Its.Iasc.Workflows.Utils
{
    public static class UtilsHelm
    {
        private static string srcDir = ".";
        private static readonly string defaultHelmCmd = "helm";
        private static string helmCmd = defaultHelmCmd;
        private static string helmAddArg = "repo add {0} {1}";
        private static string helmTplArg = "template {0} {1}/{2} {3} {4} --version {5}";
        private static string helmTplArgOci = "template {0} {1} {2} {3} --version {4}";

        public static void SetCmd(string cmd)
        {
            helmCmd = cmd;
        }

        public static void SetSourceDir(string sourceDir)
        {
            srcDir = sourceDir;
        }

        public static void ResetHelmCmd()
        {
            helmCmd = defaultHelmCmd;
        }

        public static void HelmAdd(Infra cfg)
        {
            string arg = string.Format(helmAddArg, cfg.Alias, cfg.ChartUrl);

            var prms = new List<string>();
            foreach (CmdParam param in cfg.ChartParams)            
            {
                if (param.NoValue)
                {
                    prms.Add(String.Format("--{0}", param.Name));
                }
                else
                {
                    prms.Add(String.Format("--{0}={1}", param.Name, param.Value));
                }
            }

            if (prms.Count > 0)
            {
                var extraArgs = String.Join(" ", prms.ToArray());
                arg = string.Format("{0} {1}", arg, extraArgs);
            }

            Utils.Exec(helmCmd, arg);
        }

        private static string GetTemplateName(Infra cfg)
        {
            if (!string.IsNullOrEmpty(cfg.Template))
            {
                return cfg.Template;
            }

            return cfg.ChartId;
        }

        public static string IsHelmOci(Infra cfg)
        {
            string url = cfg.ChartUrl;
            return url.Contains("oci://");
        }

        public static string HelmTemplate(Infra cfg)
        {
            var valueFiles = new List<string>();
            foreach (string file in cfg.ValuesFiles)
            {
                string path = String.Format("-f {0}/{1}", srcDir, file);
                valueFiles.Add(path);
            }
            string valueFilePath = String.Join(" ", valueFiles.ToArray());

            var values = new List<string>();
            foreach (string value in cfg.Values)
            {
                values.Add(value);
            }
            string valuesList = String.Join(" ", values.ToArray());

            string tplName = GetTemplateName(cfg);

            string arg = string.Format(helmTplArg, tplName, cfg.Alias, cfg.ChartId, valueFilePath, valuesList, cfg.Version);
            if (IsHelmOci(cfg))
            {
                arg = string.Format(helmTplArgOci, tplName, cfg.ChartUrl, valueFilePath, valuesList, cfg.Version);
            }

            if (!String.IsNullOrEmpty(cfg.Namespace))
            {
                arg = String.Format("{0} --namespace {1}", arg, cfg.Namespace);
            }

            string output = Utils.Exec(helmCmd, arg);

            return output;
        }        
    }
}
