# spmeta2-reverse
A library to provide reverse engineering of the existing SharePoint sites into SPMeta2 models.

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

// reverse the SharePoint site intp M2 model
var result = service.ReverseSiteModel(context, ReverseOptions.Default);

// here we go, your M2 model backed for you
// deploy later to other SharePoint site, farm or serialize and keep it for the future
var model = result.Model;

```
Stay tuned, releasing fiest versions Feb, 2016. 

#### Feature requests, support and contributions
In case you have unexpected issues or keen to see new features please contact support on SPMeta2 Yammer or here at github:

* https://www.yammer.com/spmeta2feedback
