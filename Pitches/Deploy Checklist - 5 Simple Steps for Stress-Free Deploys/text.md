# Deploy Checklist: 5 Simple Steps for Stress-Free Deploys

After months (or sometimes years) of sweat and toil, it can be an exciting moment when you can finally launch your application and see how it fares in the real world.  Yet, that level of excitement can also bring about a good deal of stress, as unforeseen issues may arise, delaying the official launch, or worse, causing problems and crashes for your actual users.

Proper deployment techniques can be detailed and are largely platform-dependent, so rather than focus on one particular language, tool, or method, in this article we'll aim to provide you with a few simple steps, in order to ease your application into a live, production environment.

## 1. Gather Your Tools

As the coding portion of your software development life cycle is coming to an end and your team gears up for deployment, it's time to start considering what is required of the production environment, so it can properly support your application.  The first step in this process is gathering the right tools for the job.  In most modern applications, you'll want to take advantage of some form of `continuous integration` or `automated deployment` tool.  There are many to choose from, so spend a bit of time researching and comparing the capabilities of the tool with your project needs.

- [Jenkins](https://jenkins.io): An open source `continuous integration` tool written in Java, which features `pipelines as code` and numerous plugins, allowing for simplified deployment.
- [Chef](https://www.chef.io/): Using the aptly-named `cookbooks`, Chef allows for easy manipulation of servers and deployment environments.
- [Octopus Deploy](https://octopus.com/): Automated deployment tool that focuses on `.NET` and the Windows environment.
- [Travis CI](https://travis-ci.org/): A `continuous integration` tool that can sync with code repositories to quickly and automatically test and deploy.
- [Codeship](https://codeship.com/): A customizable, hosted `continuous integration` platform service.
- [Capistrano](http://capistranorb.com/): A remote server automation tool, created in Ruby.

## 2. Provision the Staging Server

With tools in place, you'll next need to provision your staging server, including the software and components it will require.

The first decision is the operating system, which is likely to be a form of Unix or Microsoft Windows, depending on the needs of your application.  From there, you'll also want to configure the `staging` server with the correct web server and framework, also dependant on your application.

### Web Servers

Here are a few of the most popular and common web servers:

- **Apache HTTP Server**: One of the pioneers, a very popular HTTP web server.
- **Nginx**: A efficient, Unix-based HTTP web server.
- **Node.js**: JavaScript runtime that allows for the creation of an HTTP server.
- **Apache Tomcat**: A Java-based web server.
- **Gunicorn**: Python WSGI HTTP web server for Unix.
- **Microsoft IIS**: Unlike most other web servers, which typically reside on Unix systems, IIS focuses on a Windows environment.

### Framework

If you are using a framework, it will already be well-established from the initial planning and certainly during development, since it will directly impact the functionality of your code and behavior of your application.  Thus, we won't go into much detail here, but here are a few of the common frameworks, based on the underlying programming language:

- **JavaScript**: `Angular`, `React`, `Vue`, `Ember`, `Meteor`, `Express`, `Sails`, and `MEAN`.
- **Python**: `Django`, `Flask`, `Pyramid`, and `Turbogears`.
- **.NET**: .NET is arguably a framework unto itself, but for ASP.NET the `ASP.NET MVC` is also a common choice.
- **PHP**: `Laravel`, `Symfony`, `CakePHP`, `Zend Framework`, and `FuelPHP`.
- **Ruby**: `Ruby on Rails`, `Sinatra`, `Padrino`, `Cuba`, `Volt`, and `Cramp`.

## 3. Enable Service Integrations

Most modern applications must integrate with other, third-party services, so this is the next step in the deployment process.  This includes services like your content delivery network, database, automated testing, analytics, monitoring services, and so forth.  For applications which are using third-party hosting services (such as Amazon Web Services or Microsoft Azure), this is also the step where those servers would be provisioned and prepared for launch.

This step often includes slight alterations to source code or configuration files, to pass along API keys and other credentials to the automated scripts you've put in place to manage your deployment.  Therefore, this is also the perfect time to integrate an error monitoring service like [`Airbrake.io`](https://airbrake.io/).  Once your application is pushed to staging (or even onto production), error monitoring software is a simple and cost-effective way to be automatically alerted of any errors or issues that may arise in your deployed application.  Best of all, with just a few lines of code the service can be integrated into your application, informing your team of any issues, without the need for outside assistance or user feedback.

## 4. Perform User Acceptance Testing (UAT)

For most applications, the final step before deploying to production is to deploy to a `staging` environment.  The purpose of this environment is to exactly replicate the environment of `production`, while still providing limited, private access in which proper user acceptance testing (`UAT`) can be performed.  As much as possible, the staging environment should simulate the "real world" environment of production.

Once your application is deployed to `staging`, `UAT` should begin in earnest.  While the full details of proper user acceptance testing are beyond the scope of this article, there are a few general questions you need to answer, to ensure the application is ready for production:

- **Does the application crash?**  If so, determine if the problem is within the application or within an integration of some kind.
- **Are there any errors?**  If so, they should be fixed and a new build should be deployed to staging once again.
- **Is the resource usage acceptable?**  If not, perform more analytics or potentially alter the planned staging/production environment to suit the application needs.
- **Is performance acceptable?**  If not, create proper bug reports and allow for ample development time to improve performance where necessary, then deploy a new build.
- **Does the application behave as expected?**  If not, typically a bug or error is the culprit, so resolve that and try again.

At the core, this entire process should focus on the `user experience` -- can an average user use the application as expected, and get the desired results?  If anything abnormal prevents this, from the perspective of the user, then something is broken and must be resolved, before redeployment and retesting.

## 5. Deploy to Production

Once the tools are configured, the staging server is prepared, all third-party services and components are integrated, and thorough user acceptance testing has been performed, your application is finally ready for deployment to production.  While this may simply be a matter of flipping a switch, depending on the exact setup your application is utilizing, in most cases it will involve executing the scripts or commands from the deployment tools you're using, but targetting `production` instead of `staging`.

At this point, proper monitoring (of errors, performance, and like) is critical, to ensure all services behave as expected and the application doesn't come crashing down around users.  Still, if everything has gone according to plan, you're all done, and your application should be live and ready for public use!