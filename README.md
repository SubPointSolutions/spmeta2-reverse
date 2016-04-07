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

### Current coverage support per definition
<div class="m-reverse-report-cnt">
  <div>
    <h4>ContentTypeDefinition</h4>
    <table>
      <thead>
        <td>Property</td>
        <td>Support</td>
        <td>Comments</td>
      </thead>
      <tbody>
        <tr>
          <td>Description</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>DescriptionResource</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>DocumentTemplate</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Group</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Hidden</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Id</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>IdNumberValue</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Name</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>NameResource</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>ParentContentTypeId</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>ParentContentTypeName</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>ReadOnly</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Sealed</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
      </tbody>
    </table>
  </div>
  <div>
    <h4>ContentTypeFieldLinkDefinition</h4>
    <table>
      <thead>
        <td>Property</td>
        <td>Support</td>
        <td>Comments</td>
      </thead>
      <tbody>
        <tr>
          <td>DisplayName</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>FieldId</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>FieldInternalName</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Hidden</td>
          <td>True</td>
          <td>Supported, not tested yet</td>
        </tr>
      </tbody>
    </table>
  </div>
  <div>
    <h4>FeatureDefinition</h4>
    <table>
      <thead>
        <td>Property</td>
        <td>Support</td>
        <td>Comments</td>
      </thead>
      <tbody>
        <tr>
          <td>Enable</td>
          <td>False</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Id</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Scope</td>
          <td>True</td>
          <td>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
  <div>
    <h4>FieldDefinition</h4>
    <table>
      <thead>
        <td>Property</td>
        <td>Support</td>
        <td>Comments</td>
      </thead>
      <tbody>
        <tr>
          <td>AddFieldOptions</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>AdditionalAttributes</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>AddToDefaultView</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>AllowDeletion</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>DefaultValue</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Description</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>DescriptionResource</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>EnforceUniqueValues</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>FieldType</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Group</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Hidden</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Id</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Indexed</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>InternalName</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>JSLink</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>RawXml</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Required</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>ShowInDisplayForm</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>ShowInEditForm</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>ShowInListSettings</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>ShowInNewForm</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>ShowInVersionHistory</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>ShowInViewForms</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>StaticName</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Title</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>TitleResource</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>ValidationFormula</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>ValidationMessage</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
      </tbody>
    </table>
  </div>
  <div>
    <h4>ListDefinition</h4>
    <table>
      <thead>
        <td>Property</td>
        <td>Support</td>
        <td>Comments</td>
      </thead>
      <tbody>
        <tr>
          <td>ContentTypesEnabled</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>CustomUrl</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Description</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>DescriptionResource</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>DocumentTemplateUrl</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>DraftVersionVisibility</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>EnableAttachments</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>EnableFolderCreation</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>EnableMinorVersions</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>EnableModeration</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>EnableVersioning</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>ForceCheckout</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Hidden</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>IndexedRootFolderPropertyKeys</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>IrmEnabled</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>IrmExpire</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>IrmReject</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>MajorVersionLimit</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>MajorWithMinorVersionsLimit</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>NoCrawl</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>OnQuickLaunch</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>TemplateName</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>TemplateType</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Title</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>TitleResource</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Url</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>WriteSecurity</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
      </tbody>
    </table>
  </div>
  <div>
    <h4>ListViewDefinition</h4>
    <table>
      <thead>
        <td>Property</td>
        <td>Support</td>
        <td>Comments</td>
      </thead>
      <tbody>
        <tr>
          <td>Aggregations</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>AggregationsStatus</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>ContentTypeId</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>ContentTypeName</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>DefaultViewForContentType</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Fields</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Hidden</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>InlineEdit</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>IsDefault</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>IsPaged</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>JSLink</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Query</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>RowLimit</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Scope</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>TabularView</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Title</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>TitleResource</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Type</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Url</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>ViewData</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>ViewStyleId</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
      </tbody>
    </table>
  </div>
  <div>
    <h4>QuickLaunchNavigationNodeDefinition</h4>
    <table>
      <thead>
        <td>Property</td>
        <td>Support</td>
        <td>Comments</td>
      </thead>
      <tbody>
        <tr>
          <td>IsExternal</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>IsVisible</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Title</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>TitleResource</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Url</td>
          <td>True</td>
          <td>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
  <div>
    <h4>SecurityGroupDefinition</h4>
    <table>
      <thead>
        <td>Property</td>
        <td>Support</td>
        <td>Comments</td>
      </thead>
      <tbody>
        <tr>
          <td>AllowMembersEditMembership</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>AllowRequestToJoinLeave</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>AutoAcceptRequestToJoinLeave</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>DefaultUser</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Description</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Name</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>OnlyAllowMembersViewMembership</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Owner</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
      </tbody>
    </table>
  </div>
  <div>
    <h4>SecurityRoleDefinition</h4>
    <table>
      <thead>
        <td>Property</td>
        <td>Support</td>
        <td>Comments</td>
      </thead>
      <tbody>
        <tr>
          <td>BasePermissions</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Description</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Name</td>
          <td>True</td>
          <td>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
  <div>
    <h4>TopNavigationNodeDefinition</h4>
    <table>
      <thead>
        <td>Property</td>
        <td>Support</td>
        <td>Comments</td>
      </thead>
      <tbody>
        <tr>
          <td>IsExternal</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>IsVisible</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Title</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>TitleResource</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Url</td>
          <td>True</td>
          <td>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
  <div>
    <h4>UserCustomActionDefinition</h4>
    <table>
      <thead>
        <td>Property</td>
        <td>Support</td>
        <td>Comments</td>
      </thead>
      <tbody>
        <tr>
          <td>CommandUIExtension</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>CommandUIExtensionResource</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Description</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>DescriptionResource</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Group</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Location</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Name</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>RegistrationId</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>RegistrationType</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Rights</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>ScriptBlock</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>ScriptSrc</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Sequence</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>Title</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>TitleResource</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Url</td>
          <td>True</td>
          <td>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
  <div>
    <h4>WebDefinition</h4>
    <table>
      <thead>
        <td>Property</td>
        <td>Support</td>
        <td>Comments</td>
      </thead>
      <tbody>
        <tr>
          <td>AlternateCssUrl</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>CustomWebTemplate</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Description</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>DescriptionResource</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>IndexedPropertyKeys</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>LCID</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>SiteLogoUrl</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Title</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>TitleResource</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>Url</td>
          <td>True</td>
          <td>
          </td>
        </tr>
        <tr>
          <td>UseUniquePermission</td>
          <td>False</td>
          <td>Not implemented yet</td>
        </tr>
        <tr>
          <td>WebTemplate</td>
          <td>True</td>
          <td>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</div>