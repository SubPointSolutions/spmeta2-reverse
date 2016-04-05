# SPMeta2.Reverse
A library to provide reverse engineering of the existing SharePoint sites into SPMeta2 models.

### Build status
[![Build status](https://ci.appveyor.com/api/projects/status/73pbufcanckaxnqi?svg=true)](https://ci.appveyor.com/project/SubPointSupport/spmeta2-reverse)

### SPMeta2.Reverse in details

SPMeta2.Reverse provides a simple API to generate SPMeta2 models from the existing SharePoint sites. 
As easy as that. We are aiming to reverse engineer O365, SharePoint 2016/2013 web sites and generate a valid, redeployable SPMeta2 model.
As always, full regression testing, nice coverage for the SharePoint atrifact and friendly support is included.

As for the API, that's how we see it happening:
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
Stay tuned, releasing fiest versions Feb, 2016. 

#### Feature requests, support and contributions
In case you have unexpected issues or keen to see new features please contact support on SPMeta2 Yammer or here at github:

* https://www.yammer.com/spmeta2feedback

#### Current coverage support per definition

<div class='m-reverse-report-cnt'><h3>ContentTypeDefinition</h3><table><thead><td>Property</td><td>Support</td><thead><tbody><tr><td>Name</td><td>True</td></tr><tr><td>Description</td><td>True</td></tr><tr><td>Group</td><td>True</td></tr><tr><td>GetContentTypeId()</td><td>False</td></tr></tbody></table><h3>FeatureDefinition</h3><table><thead><td>Property</td><td>Support</td><thead><tbody><tr><td>Id</td><td>True</td></tr><tr><td>Scope</td><td>True</td></tr><tr><td>Enable</td><td>False</td></tr></tbody></table><h3>FieldDefinition</h3><table><thead><td>Property</td><td>Support</td><thead><tbody><tr><td>Title</td><td>True</td></tr><tr><td>Description</td><td>False</td></tr><tr><td>Id</td><td>True</td></tr></tbody></table><h3>ListDefinition</h3><table><thead><td>Property</td><td>Support</td><thead><tbody><tr><td>Title</td><td>True</td></tr><tr><td>Description</td><td>True</td></tr><tr><td>CustomUrl</td><td>True</td></tr><tr><td>TemplateType</td><td>True</td></tr><tr><td>Hidden</td><td>True</td></tr><tr><td>ContentTypesEnabled</td><td>True</td></tr><tr><td>OnQuickLaunch</td><td>True</td></tr></tbody></table><h3>ListViewDefinition</h3><table><thead><td>Property</td><td>Support</td><thead><tbody><tr><td>Title</td><td>True</td></tr></tbody></table><h3>QuickLaunchNavigationNodeDefinition</h3><table><thead><td>Property</td><td>Support</td><thead><tbody><tr><td>Title</td><td>True</td></tr><tr><td>Url</td><td>False</td></tr><tr><td>IsExternal</td><td>True</td></tr></tbody></table><h3>SecurityGroupDefinition</h3><table><thead><td>Property</td><td>Support</td><thead><tbody><tr><td>Name</td><td>True</td></tr><tr><td>Description</td><td>True</td></tr></tbody></table><h3>SecurityRoleDefinition</h3><table><thead><td>Property</td><td>Support</td><thead><tbody><tr><td>Name</td><td>True</td></tr><tr><td>Description</td><td>True</td></tr></tbody></table><h3>TopNavigationNodeDefinition</h3><table><thead><td>Property</td><td>Support</td><thead><tbody><tr><td>Title</td><td>True</td></tr><tr><td>Url</td><td>False</td></tr><tr><td>IsExternal</td><td>True</td></tr></tbody></table><h3>UserCustomActionDefinition</h3><table><thead><td>Property</td><td>Support</td><thead><tbody><tr><td>Name</td><td>True</td></tr><tr><td>Title</td><td>True</td></tr><tr><td>ScriptBlock</td><td>True</td></tr><tr><td>ScriptSrc</td><td>True</td></tr><tr><td>Location</td><td>True</td></tr><tr><td>Sequence</td><td>True</td></tr><tr><td>Url</td><td>True</td></tr><tr><td>RegistrationId</td><td>True</td></tr><tr><td>RegistrationType</td><td>True</td></tr><tr><td>Description</td><td>True</td></tr><tr><td>Group</td><td>True</td></tr></tbody></table><h3>WebDefinition</h3><table><thead><td>Property</td><td>Support</td><thead><tbody><tr><td>Title</td><td>True</td></tr><tr><td>Description</td><td>True</td></tr><tr><td>WebTemplate</td><td>True</td></tr><tr><td>Url</td><td>True</td></tr></tbody></table></div>