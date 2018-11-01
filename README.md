# segal-search
This is how we add examine search to our umbraco sites  - don't at me :)
## Naming is important
We need to come to an understanding on our team about naming - if you are referencing a site that we have already built, please acknowledge the names **AND MORE IMPORTANTY** the **Aliases**. Below is a list of our common properties - **Note:** these are aliases listed below - becaues those are the most important!
1. bodyContent - this is the alias of our often used Segal Content Grid dataType (as aliased on a document type mixin). Also be sure to follow previous conventions for the aliases of our grid editors being made with leBlender as well - see the helper.cs commentary below.
2. hideShowTitle - alias for hide show title
3. hideShowBody - alias for hide show body content
4. calloutHeadline - alias for callout headline
5. calloutText - alias for callout body content
6. bodyText - this shoudl be used as the alias for generic RTE content on a docType
7. pageHeading - generic page heading alias for use on NON content specific nodes 

**NOTE:** you can *alias*, for example, the question field on an FAQitem as **pageHeading** and the answer as **bodyText** as long as you don't have the main content mixin attached as well. The key is to minimize the customization on the nodegathering event - see the App_Code/helpers.cs file 
### Things we need to do
1. Install skybrud grid package into your umbraco install - https://our.umbraco.com/packages/developer-tools/skybrudumbracogriddata/
2. Put the skybrud leblender files from this bin... into your bin folder
3. put the itextsharp.dll and UmbracoExamine.PDF.dll into your bin as well

## Set Up Examine
Reference the ExamineIndex.config and the ExamineSettings.config in the config folder from this project. **Some things to note:** Check your index set names and be sure they make sense for your project - the samples reference **UAS** - you should update all these instances. Also take note of the features in the index sets - there are pretty good examples in there. If you have questions... ASK!
1. ExamineSettings.config
2. ExamineIndex.config

## Set up your helpers.cs
Take a look at the sample file and notice what all is happening here:
1. you should change the name of your namespace on line 27 to make sense for your project
2. on line 43, you need to update the name of the index you want to pump all this info into
3. start looking on line 57 and down to see how important naming is... if you selected your own naming convention and aliases... all this code will have to be updated.
4. note that editor aliases are important as well.
5. there are helpers in this file that you **DO NOT** need for your project. Please only grab what you need.

## Finalization and Testing
Once you get all this stuff in place, you should go to the developer section and go into the examine management tab. You shoudl build/rebuild the indexes you set up just to make sure they have all the nodes and properties that you expect. Note the the multindexer/multisearcher does not really work back here as this is custom. HOWEVER, you can check the content and pdf indexes and searches separately back here because we set up independent indexes and searchers to test.

Hit me up if you have questions!