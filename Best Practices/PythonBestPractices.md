Like most programming languages, Python offers an extremely powerful development platform to create some of the most useful and robust applications imaginable.  However, as Winston Churchill and even early Spider-Man have taught us, _with great power comes great responsibility._

Today we'll examine a few of the critical Python best practices used to create more professional, cleaner code.

## Properly Structure Your Repository

Most developers, no matter the language, will begin a new project by settings up a code repository and some form of version control.

While endless debates can (and do) take place arguing about which version control system is best or where to host your project repositories, no matter what you decide to use, it's critical to focus on structuring your project and subsequent repository in a suitable fashion.

For Python, in particular, there are a few key components that should exist within your repository, and you should take the time to generate each of these, at least in a basic skeletal form, before actual coding begins.

- __License__ - `[root]`: This file should be the first thing you add to your project.  If you're uncertain, sites like [choosealicense.com](http://choosealicense.com/) can help you decide.
- __README__ - `[root]`: Whether you choose Markdown, reStructuredText, or even just plain text as your format, getting a basic `README` file in place can help you describe your project, define its purpose, and outline the basic functionality.
- __Module Code__ - `[root]` or `[/module]`: Of course this is all for naught if you don't have any new or worthwhile code to create.  That said, be sure to place your actual code inside a properly-named sub-directory (e.g. `/module`), or for a single-file project, within `root` itself.
- __setup.py__ - `[root]`: A basic `setup.py` script is very common in Python projects, allowing `Distutils` to properly build and distribute the modules your project needs.  See the [official documentation](https://docs.python.org/3.6/distutils/setupscript.html) for more information on `setup.py`.
- __requirements.txt__ - `[root]`: While not all projects utilize a `requirements.txt` file, it can be used to specify development modules and other dependencies that are required to work on the project properly.  See the [official PIP page](https://pip.pypa.io/en/stable/user_guide/#requirements-files) for details.
- __Documentation__ - `[/docs]`: Documentation is a key component to a well-structured project, and typically it is placed in the `/docs` directory.
- __Tests__ - `[/tests]`: Just like documentation, tests also typically exist in most projects, and should be placed in their own respective directory.

All told, this means a basic Python repository structure might look something like this:

```
docs/conf.py
docs/index.rst
module/__init__.py
module/core.py
tests/core.py
LICENSE
README.rst
requirements.txt
setup.py
```

## Follow Common Style Guidelines

Python has a system of community-generated proposals known as [Python Enhancement Proposals](https://www.python.org/dev/peps/) (PEPs) which attempt to provide a basic set of guidelines and standards for a wide variety of topics when it comes to proper Python development.

Perhaps one of the most widely known and referenced PEPs ever created is [`PEP8`](https://www.python.org/dev/peps/pep-0008/), which is the "Python community Bible" for properly styling your code.

While there are far too many specific styling guidelines to give examples here, most Python developers make a concerted effort to follow the guidelines set forth in `PEP8`, so all code across the community has a similar look and feel.

## Immediately Repair Your Broken Windows

Perhaps you've heard of the [`broken windows theory`](https://en.wikipedia.org/wiki/Broken_windows_theory), which is a criminological theory that states that the proper maintenance and monitoring of urban environments in cities, in order to prevent lesser crimes, creates a more positive atmosphere that thereby prevents greater crimes from occurring.

While this theory, as it relates to crime prevention, is _highly_ criticized and contested, it has some merit when it comes to the world of coding and application development.

Specifically, when creating a Python application (or any language for that matter), it is _almost always_ more beneficial in the long-term to quickly acknowledge and repair broken windows (i.e. broken code) immediately.  Putting it off to finish a new component or module can and will often lead to further, more complex problems down the line.

It has [even been stated](http://www.joelonsoftware.com/articles/fog0000000043.html) that Microsoft, after a terrible production cycle with the original version of Microsoft Word, adopted a `"zero defects methodology"` to their great benefit, always focusing on fixing bugs and defects before new production code.  While this accounting may be apocryphal, there is a certain logic to applying this model during development.

## Create Consistent Documentation

As much as it can feel like a burden at times, proper documentation throughout the lifecycle of a project is a cornerstone to clean code.  Thankfully, the Python community has made this process fairly painless, and it involves the use of three simple tools and concepts: [`reStructredText`](http://docutils.sourceforge.net/rst.html), [`Docstrings`](https://www.python.org/dev/peps/pep-0257/), and [`Sphinx`](http://www.sphinx-doc.org/).

[`reStructredText`](http://docutils.sourceforge.net/rst.html) is a simple plain text markup syntax.  It is commonly used for in-line documentation, which in the case of Python, allows for documentation to be generated on-the-fly.  The [`Quickstart Guide`](http://docutils.sourceforge.net/docs/user/rst/quickstart.html) provides a few simple examples of `reStructuredText` in action.

While using `reStructuredText` is the first step to proper documentation, it's critical to understand how to properly generate [`Docstrings`](https://www.python.org/dev/peps/pep-0257/).  A `Docstring` is simply a documentation string that appears prior to every module, class, or method within your Python code.

Once every component of your code contains a properly formatted `Docstring` using `reStructuredText` markdown, you can move onto using [`Sphinx`](http://www.sphinx-doc.org/), which is the go-to tool to generate Python documentation from existing `reStructuredText`.  `Sphinx` allows documentation to easily be exported into a variety of formats, including beautiful HTML, for near-automatic online documentation pages.  With virtually no effort, your project documentation can even be added to the prominent [`ReadTheDocs`](https://docs.readthedocs.io/en/latest/getting_started.html#import-your-docs) documentation repository after every code commit.

## Get Acquainted With PyPI

While using proper syntax and documentation methods will always produce solid, clean code, perhaps one of the best tools to improving your use of Python is the epic module repository known as [`PyPI: The Python Package Index.`](https://pypi.python.org/pypi).  As a collective repository for thousands of Python projects, including nearly every open or public Python project created, `PyPI` is an undeniably useful resource, no matter your Python experience level or project scope.

There are two primary uses for `PyPI`: Using existing modules, or adding your own project to `PyPI`.

Most projects will initially begin by utilizing existing projects on `PyPI`, and with nearly `100,000` at the time of writing to choose from, there's almost certainly some code that fits your project's needs.  Once you've located a module you're interested in using within your own project, the fun can begin.

Next, you'll want to utilize the `Python Packaging Authority` [recommendations for installing a package](https://packaging.python.org/current/) from `PyPI`.  Typically this entails using [`pip`](https://pypi.python.org/pypi/pip), the go-to package installation software.  If you're using `Python 2 >=2.7.9` or `Python 3 >=3.4`, `pip` comes pre-installed with Python.

Once `pip` is installed, simply run the appropriate command, as suggested by the module documentation within `PyPI`.  For example, to install [`requests`](https://pypi.python.org/pypi/requests/2.12.1), a common module to handle HTTP requests, you'd simply use the following command from the `root` directory of your project:

```
pip install requests
```

`pip` will then take care of the rest and install the package.  Now you can keep adding more modules to help your code shine!

Down the road, if you've got a decent module of your own created and wish to share it with the rest of the Python community, you'll want to take a look at the [`Python Distribution`](https://packaging.python.org/distributing/) documentation, which handles the finer details of packaging your project, as discussed earlier.

Most importantly, look over the simple steps to [`Upload Your Project to PyPI`](https://packaging.python.org/distributing/#uploading-your-project-to-pypi), which requires you to: Signup for a `PyPI` account, register your project, and then upload it to `PyPI` for others to view and add to their own projects!

---

__SOURCES__

- http://docs.python-guide.org/en/latest/writing/structure/#structure-of-the-repository
- https://www.python.org/dev/peps/pep-0008/
- https://en.wikipedia.org/wiki/Broken_windows_theory
