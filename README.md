HandlebarsHelper
================

HandlebarsHelper will ultimately help you inject your handlebars files into your page.

## Quick Start Guide

The most common use case is you'd like to bundle precompiled/minified EmberJS Handlebar templates.

### In the BundleConfig 
For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725

``` csharp

public class BundleConfig
{
  public static void RegisterBundles(BundleCollection bundles)
  {

    //other bundles
  
    // ~/bundles/templates is the bundle name
    // ~/Scripts/templates is the virtual path to your template files
    bundles.Add(new Bundle("~/bundles/templates", new HandlebarsTransformer())
           .IncludeDirectory("~/Scripts/templates", "*.hbs", true)
            );

    BundleTable.EnableOptimizations = true;
  }
}
```

### In your view

``` csharp
// this should be after the Ember framework
@Scripts.Render("~/bundles/templates")
```

### Done


## Debug Mode

This use case is you want the templates injected into the page, but not precompiled/minified/bundled, just inject em

### In your view (Razor)

``` csharp
using HandlebarsHelper;
   
// this should be after the Ember framework
// "~/scripts/templates" is the virtual path to the templates
//  new[] { "*.hbs" } is the filter extension array (add as many as you want)
@HandlebarsHelper.RawTemplateInjector.InjectRawTemplates("~/scripts/templates", new[] { "*.hbs" })
```

### Done

## Template naming

Template naming is an area of controversy, I have my opinion, you have yours, and mine is right ;)

### How I name the files

There are a few different scenarions that render differently

<table>
    <tr>
   <td>Path from included directory</td>
   <td>File name</td>
   <td>Template name</td>
   <td>Scenario</td>
    </tr>
    <tr>
   <td>/</td>
   <td>apple.hbs</td>
   <td>apple</td>
   <td>file in the root</td>
    </tr>
    <tr>
   <td>/blah/</td>
   <td>blah.hbs</td>
   <td>blah</td>
   <td>file in folder and matches the folder name(exact match)</td>
    </tr>
    <tr>
   <td>/blah/</td>
   <td>cow.hbs</td>
   <td>blah/cow</td>
   <td>file in folder, and doesn't match the folder name</td>
    </tr>
    <tr>
   <td>/dog/</td>
   <td>Dog.hbs</td>
   <td>dog/Dog</td>
   <td>file in folder, and doesn't match the folder name</td>
    </tr>
    <tr>
   <td>/dog/</td>
   <td>elephant.hbs</td>
   <td>dog/elephant</td>
   <td>file in folder, and doesn't match the folder name</td>
    </tr>
</table>

#### You hate how I implemented the naming, cause you like how you did it, despite being wrong

That's cool, The `HandlebarsTransformer` and `RawTemplateInjector` take an optional parameter which allow you to define a class that names your templates!

Create a new class and implement the `ITemplateNamer` interface

``` csharp
public interface ITemplateNamer
{
  string GenerateName(string bundleRelativePath, string fileName);
}
```

What is passed in?  If you look up above, at the table, column 2 and 3, that's it.  The template's relative path and the template's file name.

Example custom namer:

``` csharp
public class CustomTemplateNamer : ITemplateNamer
{
  public string GenerateName(string bundleRelativePath, string fileName)
  {
    var fileNameNoExtension = Path.GetFileNameWithoutExtension(fileName);
    return "moooooooooooo" + fileNameNoExtension;
  }
}
```

##### Bundled: In your BundleConfig

``` csharp
bundles.Add(new Bundle("~/bundles/templates", new HandlebarsTransformer(new CustomTemplateNamer()))
       .IncludeDirectory("~/Scripts/templates", "*.hbs", true)
       );
```


##### Raw: In your View 

``` csharp
  @HandlebarsHelper.RawTemplateInjector.InjectRawTemplates("~/scripts/templates", 
                                                           new[] { "*.hbs" }, 
                                                           new CustomTemplateNamer());
```

Report all issues, feel free to submit PRs, clean up the code, make the readme more coherent, stylize my example page.
