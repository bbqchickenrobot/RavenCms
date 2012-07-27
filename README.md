#RavenCms
RavenCms is a simple ASP.NET MVC 4 CMS for small Projects that is based on RavenDb that uses this advantages.
There is already a CMS by the RavenDb creator Ayende called [RaccoonBlog](https://github.com/fitzchak/RaccoonBlog). But it was to specific for my needs.

#Vision
Why another CMS you ask? RavenCms most significant and unique feature is the UX of how content is managed: 

##There is now backend!
This is achieved by the very powerful HTML5 [contenteditable](http://html5doctor.com/the-contenteditable-attribute/) feature that is even supported by IE6.

Content is the center of the Website and there is nothing more natural than editing it inplace.
There are two editors out there that take advantage of this feature and I consider to use:

- [Raptor Editor](http://www.raptor-editor.com/)
- [Mercury Editor](http://jejacks0n.github.com/Mercury)

#Design Principles
There are also a few principles I set myself:

- Simplicity as a feature
- Low learning curve for developers
- Convention over configuration
- Take advantages of ASP.NET MVC 4 features
- No CMS infrastructure code

#Implementation Details

## Use OSS
It is very important that we don't reinvent the wheel.
This is why I take as much advantage of the open source projects given and try to implement as little as possible:

- [RavenDb](http://ravendb.net/) and the schemeless nature of NoSQL Documents
- [Automapper](https://github.com/AutoMapper/AutoMapper)
- [MvcContrib](http://mvccontrib.codeplex.com/)
- [Autofac] (http://code.google.com/p/autofac/)

## Editable - but Addable? 
The contenteditable feature makes it very easy to edit html. But what about adding new elements?
This is where example elements kick in:
If you want to add a post, you already see a post filled with default content that you just have to edit and save.

## Controller Structure

By convention an editable and addable object needs only 3 methods in a controller.
You probably miss the edit method. This is because there is no need for one.
All elements can be edited in their final form, you only have one place (Save) where you have to care how persist your data.

### Show (GET)
The show method returns a view and a view model rendered in state you want to present the content to the user.
### Add (GET)
This method creates an example view model and renders it like the show method.
### Save (POST)
This method stores the edited or added view model.

## Validation ##
As the content always stays in its final presentation you won't have any forms.
This is very special approach but we can still use the powerful validation capabilities of ASP.NET MCV.

- In the CMS all View Model Validation happens via the validation attributes - this is best practice.
- All validation happens on the server as the value is posted anyway we will retrieve a view that we will render containing information about the state of the elment
- If you have special validation requirements consider creating a new view model and not a new entity model




