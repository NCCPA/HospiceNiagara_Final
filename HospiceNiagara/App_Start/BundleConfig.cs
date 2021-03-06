﻿using System.Web;
using System.Web.Optimization;

namespace HospiceNiagara
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-1.11.4.js",
                        "~/Scripts/jquery.timepicker.js",
                        "~/Scripts/fileinput.js",
                        "~/Scripts/jquery.bootstrap.wizard.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/fullcalendar.js",
                        "~/Scripts/custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/jquery.timepicker.css",
                      "~/Content/themes/base/datepicker.css",
                      "~/Content/themes/base/theme.css",
                      "~/Content/themes/base/core.css",
                      "~/Content/bootstrap-fileinput/css/fileinput.css",
                      "~/Content/fullcalendar.css",
                      "~/Content/site.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
