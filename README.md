# Asp.Net-Assignment
A blog post website developing using asp.net core framework in C#

Author: Dongxu Jiang

Language, Frameworks:
  * C#
  * Razor
  * Bootstrap

Layout:
  * wwwroot
    - folder of bootstrap css files, used for page styling
  * Controllers
    - homecontroller with all functions to process request and perform response
  * Models
    - Assigment1DbContext: A db context class which collects all abset of models
    - BlogPost: the model of blogpost, propety includes BlogPostId, UserId, Title, Content, Posted, user, comment
    - Comment: the model of comment, propety includes CommentId, BlogPostId, UserId, Content, User
    - Role: the model of role, propety includes RoleId, Name
    - User: the model of user, propety includes UserId, RoleId, FirstName, LastName, EmailAddress, Password
  * Views
    - Home (folder)
      - AddBlogPost.cshtml, web view page for user to create new blog post, integrated function is AddPosted() from homecontrller
      - DisplayFullBlogPost.cshtml, web view page to display a full blogpost with title, full content, posted time, user comments, and leave comment area, integrated function is AddComment() and Login() from homecontrller
      - EditBlogPost.cshtml, web view page to allow admin user to edit a selected blog, integrated function is ModifyBlog() from homecontroller
      - Index.cshtml, web view page of home, display each blog post in a card with limited characters of contents, integrated function is DisplayFullBlogPost(), EditBlogPost(), DisplayFullBlogPost() from homecontroller
      - Login.cshtml, web view page to allow user login, integrated function is Verify() from homecontroller
      - Register.cshtml, web view page to allow vister to create an account, integrated function is CreateUser() from homecontroller
      - Search.cshtml, web view page to allow vister to search for blogs based on the partial title/content entered by visiter.
    - Shared
      - _Layout.cshtml, web view of header which is shared by all other pages above
    - _ViewImports.cshtml, through adding addTagHelper to enable server-side code to participate in creating and rendering HTML elements in Razor files

Hosting Address: https://ttpost.azurewebsites.net


