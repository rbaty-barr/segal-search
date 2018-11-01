using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using umbraco;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Examine;
using Examine.Providers;
using Skybrud.Umbraco.GridData;
using Skybrud.Umbraco.GridData.Values;
using Skybrud.Umbraco.GridData.Interfaces;
using Skybrud.Umbraco.GridData.LeBlender.Values;
using Lecoati.LeBlender.Extension.Models;
using Newtonsoft.Json;

namespace uas.App_Code
{
    public class Startup : ApplicationEventHandler {

        private static ExamineIndexer _examineIndexer;

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) {
            // Register events for Examine
            _examineIndexer = new ExamineIndexer();
        }

    }

    public class ExamineIndexer {
        public ExamineIndexer() {

            BaseIndexProvider externalIndexer = ExamineManager.Instance.IndexProviderCollection["uasIndexer"];

            externalIndexer.GatheringNodeData += OnExamineGatheringNodeData;

        }

        private void OnExamineGatheringNodeData(object sender, IndexingNodeDataEventArgs e) {
            try {
                string nodeTypeAlias = e.Fields["nodeTypeAlias"];

                LogHelper.Info<ExamineIndexer>("Gathering node data for node #" + e.NodeId + " (type: " + nodeTypeAlias + ")");
                string value;

                
                        GridDataModel grid = GridDataModel.Deserialize(e.Fields["bodyContent"]);
                        StringBuilder hsText = new StringBuilder();
                        StringBuilder basicText = new StringBuilder();
                        StringBuilder caText = new StringBuilder();
                        StringBuilder headlineText = new StringBuilder();
                        
                        string html ="";
                        string text = "";

                        foreach (GridControl ctrl in grid.GetAllControls()) {
                                if(ctrl.Editor.Alias == "headline"){
                                            LogHelper.Info<ExamineIndexer>("We found a headline");
                                        
                                            html = ctrl.GetValue<GridControlTextValue>().Value;
                                            //LogHelper.Info<ExamineIndexer>(html);
                                            text = Regex.Replace(html, "<.*?>", "");

                                            // Extra decoding may be necessary
                                            text = HttpUtility.HtmlDecode(text);
                                            // Now append the text
                                            headlineText.AppendLine(text);
                                            //LogHelper.Info<ExamineIndexer>("Going into index: " + combined.ToString());
                                            e.Fields["headlineContent"] = headlineText.ToString();
                                }

                                if(ctrl.Value is GridControlLeBlenderValue) {
                                    // Iterate over the array of LeBlender items (in the raw JSON)
                                        
                                        LogHelper.Info<ExamineIndexer>("leBlender Items Found!  " + ctrl.Editor.Alias);
                                        if(ctrl.Editor.Alias == "hideShow"){
                                            //LogHelper.Info<ExamineIndexer>("leBlender Hide Show Found!");
                                            //LogHelper.Info<ExamineIndexer>("Trying to find value: " + ctrl.Value.ToString());
                                            LogHelper.Info<ExamineIndexer>("We are looking for this many things: " + ctrl.GetValue<GridControlLeBlenderValue>().Items.Count());
                                            
                                            foreach(LeBlenderValue thing in ctrl.GetValue<GridControlLeBlenderValue>().Items){
                                                //LogHelper.Info<ExamineIndexer>("Hide Show Title: " + thing.GetRawValue("hideShowTitle"));
                                                LogHelper.Info<ExamineIndexer>("Hide Show Content: " + thing.GetRawValue("hideShowBody"));
                                                html = thing.GetRawValue("hideShowTitle") + " " + thing.GetRawValue("hideShowBody");
                                                // Strip any HTML tags so we only have text
                                                text = Regex.Replace(html, "<.*?>", "");

                                                // Extra decoding may be necessary
                                                text = HttpUtility.HtmlDecode(text);

                                                // Now append the text
                                                hsText.AppendLine(text);
                                                //LogHelper.Info<ExamineIndexer>("Going into index: " + combined.ToString());
                                                e.Fields["hsContent"] = hsText.ToString();
                                                
                                            }
                                            
                                        } 
                                        
                                        if(ctrl.Editor.Alias == "textEditor"){
                                            foreach(LeBlenderValue thing in ctrl.GetValue<GridControlLeBlenderValue>().Items){
                                                html = thing.GetRawValue("richText");

                                                // Strip any HTML tags so we only have text
                                                text = Regex.Replace(html, "<.*?>", "");

                                                // Extra decoding may be necessary
                                                text = HttpUtility.HtmlDecode(text);

                                                // Now append the text
                                                basicText.AppendLine(text);
                                                //LogHelper.Info<ExamineIndexer>("Going into index: " + combined.ToString());
                                                e.Fields["textContent"] = basicText.ToString();
                                            }
                                        } 
                                        
                                        if(ctrl.Editor.Alias == "calloutBox"){
                                            foreach(LeBlenderValue thing in ctrl.GetValue<GridControlLeBlenderValue>().Items){
                                                html = thing.GetRawValue("calloutHeadline") + " " + thing.GetRawValue("calloutText");
                                                // Strip any HTML tags so we only have text
                                                text = Regex.Replace(html, "<.*?>", "");

                                                // Extra decoding may be necessary
                                                text = HttpUtility.HtmlDecode(text);

                                                // Now append the text
                                                caText.AppendLine(text);
                                                //LogHelper.Info<ExamineIndexer>("Going into index: " + combined.ToString());
                                                e.Fields["calloutContent"] = caText.ToString();
                                            }
                                        }
                                        
                                    } 
                        }
                                
                               

                            

                        

                       

                

            } catch (Exception ex) {

                LogHelper.Error<ExamineIndexer>("MAYDAY! MAYDAY! MAYDAY!", ex);
            
            }
        }
    }


    public class getcampusSurfaceController : SurfaceController
    {
        [HttpPost]
        public ActionResult getCampus()
        {
            //var contentService = ApplicationContext.Current.Services.ContentService;
            //var homeNode = contentService.GetRootContent().FirstOrDefault();
		    //var upDoc = contentService.GetById(Convert.ToInt32(homeNode.Id));
            var campusId = Request["campus"];
            if (!string.IsNullOrWhiteSpace(campusId))
            {
		    var getInfo = Umbraco.TypedContent(Convert.ToInt32(campusId));
            var theLogo = getInfo.GetPropertyValue<IPublishedContent>("schoolLogo");	
		    var nowViewing = "<p class='text-right mb-1'><a href='#' id='changeCampus'>Change Campus <i class='fal fa-angle-right'></i></a></p>";
            nowViewing = nowViewing + "<p class='viewingText mb-1'>You are viewing content for:</p>";
            nowViewing = nowViewing + "<p id='what-campus' class='d-block d-md-none'>" + getInfo.Name + "</p>";
            if(theLogo != null){
                nowViewing = nowViewing + "<div id='campus-logo' class='d-none d-md-block'><img src='" + theLogo.Url + "' alt='" + getInfo.Name + "' class='img-fluid'/></div>";
            } else {
                nowViewing = nowViewing + "<p id='what-campus' class='d-none d-md-block'>" + getInfo.Name + "</p>";
            }
            
            
            return Content(nowViewing.ToString());
            } else {
                return Content(campusId + " - There was an error getting your campus info, please try again. <a href='#' id='changeCampus'>Change Campus <i class='fal fa-angle-right'></i></a>");
            }
        }
    }

    public class getrandomfaqSurfaceController : SurfaceController
    {
        [HttpPost]
        public ActionResult getFaq()
        {
            var homeId = Request["homeID"];
            var campusId = Convert.ToInt32(Request["campus"]);
            var faqw = Request["faqw"];

            if (!string.IsNullOrWhiteSpace(homeId))
            {
                var theFaq = "";
                var faqData = Umbraco.TypedContent(Convert.ToInt32(homeId));
                var faqHome = faqData.Site().Descendant("fAQsHome");
if(faqData.HasValue("faQPicker")){
                var faqsForSchool = faqData
                    .GetPropertyValue<IEnumerable<IPublishedContent>>("faQPicker")
                    .Select(x => 
                        new {SchoolContent= x.GetPropertyValue<IEnumerable<IPublishedContent>>("schoolSpecificContent"), x})
                    .Where(x => (x.SchoolContent!= null && x.SchoolContent.Any(y => y.Id == campusId)) || x.SchoolContent== null || (x.SchoolContent!= null && !x.SchoolContent.Any())
                    ).Select(x => x.x)
                    .RandomOrder()
                    .Take(1);

                theFaq = "<div id='faq-wrapper'>";
                theFaq = theFaq + "<div class='container'>";
                theFaq = theFaq + "<div class='row justify-content-sm-center' id='faq-row'>";
                theFaq = theFaq + "<div class='col-sm-" + faqw + "'>";
                theFaq = theFaq + "<h3 class='faq-header'>Frequently Asked Questions</h3>";
                foreach(var faq in faqsForSchool) { 
                    var theTitle = faq.GetPropertyValue("pageHeading");
                    var theBody = faq.GetPropertyValue("bodyText");

                    theFaq = theFaq + "<div class='faq-body'>";
                    theFaq = theFaq + "<p class='mb-0'><strong>" + theTitle + "</strong></p>";
				    theFaq = theFaq +  theBody; 
                     theFaq = theFaq + "</div>";
    
                }
                theFaq = theFaq + "<a href='" + faqHome.Url +"' class='btn btn-outline-light'>See all FAQs <i class='fal fa-angle-right'></i></a>";
                theFaq = theFaq + "</div>";
                theFaq = theFaq + "</div>";
                theFaq = theFaq + "</div>";
                theFaq = theFaq + "</div>";
                }

                return Content(theFaq.ToString());
            } else {
                return Content("Sorry, there is no FAQ data to show at this time");
            }
        }
    }
}