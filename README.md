# segal-search
This is how we add examine search to our umbraco sites  - don't at me :)
## Naming is important
We need to come to an understanding on our team about naming - if you are referencing a site that we have already built, please acknowledge the names **AND MORE IMPORTANTY** the **Aliases**. Below is a list of our common properties - **Note:** these are aliases listed below - becaues those are the most important!
1. bodyContent - this is the alias of our often used Segal Content Grid dataType (as aliased on a document type mixin)
2. hideShowTitle - alias for hide show title
3. hideShowBody - alias for hide show body content
4. calloutHeadline - alias for callout headline
5. calloutText - alias for callout body content
6. bodyText - this shoudl be used as the alias for generic RTE content on a docType
7. pageHeading - generic page heading alias for use on NON content specific nodes 

**NOTE:** you can *alias*, for example, the question field on an FAQitem as **pageHeading** and the answer as **bodyText** as long as you don't have the main content mixin attached as well. The key is to minimize the customization on the nodegathering event - see the App_Code/helpers.cs file 
### Things we need to do
1. Install skybrud grid package into your umbraco install - https://our.umbraco.com/packages/developer-tools/skybrudumbracogriddata/