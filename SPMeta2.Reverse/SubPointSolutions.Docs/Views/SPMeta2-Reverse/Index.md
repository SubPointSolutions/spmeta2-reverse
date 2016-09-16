---
Title: 'M2 Reverse Home'
Tile: true
TileTitle: 'M2 Reverse'
TileOrder: 40
TileLink: true
TileLinkOrder: 8
TileDescription: 'A library to provide reverse engineering of the existing SharePoint sites into SPMeta2 models.'
---

## SPMeta2 Reverse

SPMeta2.Reverse provides a simple API to generate SPMeta2 models from the existing SharePoint sites. As easy as that. We are aiming to reverse engineer O365, SharePoint 2016/2013 web sites and generate a valid, redeployable SPMeta2 model.

Auto merge CI - 4, added appveyor full build on cake

### Reverse API

The project is still in alpha with source code avialable at github:
* [https://github.com/SubPointSolutions/spmeta2-reverse](https://github.com/SubPointSolutions/spmeta2-reverse)

Reverse API should be made simple, so here is how we see it happening:

```cs
// SharePoint CSOM context - O365/SP2016/SP2013
var context = ..; 

// create the magic reverse service
var service = new CSOMReverseService();

// reverse the SharePoint site and web into M2 model
var siteModelResult = service.ReverseSiteModel(context, ReverseOptions.Default);
var webModelResult = service.ReverseWebModel(context, ReverseOptions.Default);

// here we go, your M2 models backed for you
// deploy later to other SharePoint site, farm or serialize and keep it for the future
var siteModel = siteModelResult.Model;
var webModel = webModelResult.Model;
```

Stay tuned, we'll be updating document with more samples once stables reverse API id released.

### Feature requests, support and contributions

In case you have unexpected issues or keen to see new features please contact support on SPMeta2 Yammer or here at github:

* [https://www.yammer.com/spmeta2feedback](https://www.yammer.com/spmeta2feedback)
* [https://twitter.com/spmeta2](https://twitter.com/spmeta2)

Current coverage support per definition

[Detailed coverage report](https://github.com/SubPointSolutions/spmeta2-reverse/blob/master/M2.Reverse.Coverage.Status.md)

* BooleanFieldDefinition
* ChoiceFieldDefinition
* ContentTypeDefinition
* ContentTypeFieldLinkDefinition
* CurrencyFieldDefinition
* DateTimeFieldDefinition
* FeatureDefinition
* FieldDefinition
* FolderDefinition
* GeolocationFieldDefinition
* GuidFieldDefinition
* HTMLFieldDefinition
* ImageFieldDefinition
* LinkFieldDefinition
* ListDefinition
* ListViewDefinition
* LookupFieldDefinition
* MasterPageDefinition
* MediaFieldDefinition
* ModuleFileDefinition
* MultiChoiceFieldDefinition
* NoteFieldDefinition
* NumberFieldDefinition
* OutcomeChoiceFieldDefinition
* PropertyDefinition
* QuickLaunchNavigationNodeDefinition
* SandboxSolutionDefinition
* SecurityGroupDefinition
* SecurityRoleDefinition
* SummaryLinkFieldDefinition
* TaxonomyFieldDefinition
* TaxonomyTermDefinition
* TaxonomyTermGroupDefinition
* TaxonomyTermSetDefinition
* TaxonomyTermStoreDefinition
* TextFieldDefinition
* TopNavigationNodeDefinition
* URLFieldDefinition
* UserCustomActionDefinition
* UserFieldDefinition
* WebDefinition
* WebPartPageDefinition
* WelcomePageDefinition
* WikiPageDefinition