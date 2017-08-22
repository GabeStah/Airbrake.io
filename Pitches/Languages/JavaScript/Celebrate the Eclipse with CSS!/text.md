# Celebrate the Eclipse with CSS!

Although total solar eclipses typically occur at least twice a year in some part of the world, on August 21st, 2017, many Americans will experience the first total eclipse in their lifetimes!  Last time anyone on U.S. soil saw a total eclipse was in the lovely state of Hawaii back in 1991.  Prior to that, a few states near the west coast got a glimpse in 1979, but this 2017 event will mark a total eclipse path across the entire country!

To celebrate this momentous occasion we thought we'd show you how to create your very own solar eclipse using nothing but the power of cascading style sheet (`CSS`) animations.  As you may recall, we celebrated the Fourth of July this year with a cool [`JavaScript Fireworks`](https://airbrake.io/blog/javascript/fourth-of-july-javascript-fireworks) animation, but now we're simplifying things even more by sticking with CSS for this little tutorial.  With that, let's have some fun!

## Tools You'll Need 

To follow along with this project you'll need a modern web browser, your favorite text editor, and `Ruby` installed.  For Windows users, installing Ruby is super easy with the [`RubyInstaller`](https://rubyinstaller.org/) project.  For Mac or Linux users, your favorite [`package manager`](https://www.ruby-lang.org/en/documentation/installation/) will do the trick.

## Installing Sass

While we *could* write our style sheets in modern CSS, we'll be taking advantage of `CSS variables`, which are [not completely supported by all browsers](https://developer.mozilla.org/en-US/docs/Web/CSS/Using_CSS_variables#Browser_compatibility) at this time.  Rather than asking you to use a specific browser to run this code, we'll be opting to use the powerful [`Sass`](http://sass-lang.com/) CSS extension.  `Sass` allows us to create more readable CSS, while also using variables throughout the style sheet so we can keep things organized _and_ make it easier to play around with the settings later on.

Once you have Ruby installed, installing `Sass` is just a matter of executing the `gem install sass` command from your terminal:

```bash
$ gem install sass
Successfully installed sass-3.5.1
Parsing documentation for sass-3.5.1
Installing ri documentation for sass-3.5.1
Done installing documentation for sass after 6 seconds
1 gem installed
```

Now let's move onto setting up the project!

## Project Structure

Since we're using just HTML and CSS for this tutorial, all we need are two files in our project directory:

- `index.html`: This is the HTML page we'll load in our browser to view our finished work.
- `default.scss`: The `Sass`-powered cascading stylesheet that contains all the styles we'll manipulate to create our eclipse animation.

That's it, so add a new project directory wherever you'd like and create those two blank files.  If you wish to use a third-party tool you can also opt for a development playground like [`Codepen`](https://codepen.io/pen/), in which case everything will appear in the browser as you follow along.  _Note: If you use Codepen, be sure to select `SCSS` from the CSS dropdown toggle._

The way `Sass` works behind the scenes is that you write your special, extended CSS within a `.scss` file, then `Sass` will parse and compile that advanced code into baseline CSS that all modern browsers understand.  To accomplish this we'll tell `Sass` to _watch_ a particular file, and when it detects changes, it will automatically parse and compile it into the output file we want.

To begin this process, navigate to your project directory within your console and type the following command:

```bash
$ sass --watch default.scss:default.css
>>> Sass is watching for changes.  Press Ctrl-C to stop.
```

This tells `Sass` to watch the `default.scss` file where we'll actually write our code, and to output the resulting CSS to `default.css`, which our `index.html` file will load.

## The End Goal

If you want to skip ahead and see what the end result will look like, check out [this CodePen page](https://codepen.io/GabeStah/pen/mMxxxJ).

## Creating the HTML Page

We'll start by creating the basic HTML structure with the elements we'll manipulate using CSS.  In this case, we have two basic objects, the sun and the moon.  Both of these are represented by a `div` element, which is also assigned the `eclipse` class that we'll use in our CSS to create the actual eclipse animation effects:

```html
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8" />
        <title>Eclipse 2017 in CSS!</title>
        <link href="default.css" rel="stylesheet">
    </head>
    <body>
        <div class="eclipse sun"></div>
        <div class="eclipse moon"></div>
    </body>
</html>
```

Our `index.html` file begins with a standard `DOCTYPE` specification, the `<meta>` tag where we specify the character set we're using, and the page `<title>`.  We also need to `link` our `default.css`, since, as you'll recall, this is the compiled output file that `Sass` will automatically generate for us.

## Creating the Sass Stylesheet

Now we get into the real bread and butter of the project.  The goal, as you might imagine, is to animate the trajectory of the `moon` so it passes in front of the `sun`.  As it does so, we also want to adjust the color of the sky so it darkens as the moon approaches and reaches totality.  Lastly, for a bit more realism we want to create a corona effect around the sun as the moon begins to line up during total eclipse.

That's all well and good, but we also want to be able to easily adjust things and play around with different settings.  We can accomplish this by making heavy use of [`variables`](http://sass-lang.com/documentation/file.SASS_REFERENCE.html#Variables_____variables_), which begin with a dollar sign (`$`) in `Sass` syntax.

### Configuration

We start with a number of configuration options:

```scss
// === CONFIGURATION ===

// Duration of full eclipse animation.
$duration: 12s;

// Size of the sun and moon.
$celestial-body-diameter: 400px;

// Sun color.
$sun-color: #ffad00;

// Moon color.
$moon-color: #dddddd;

// Corona color.
$corona-color: #efefef;
// Radius of the corona.
$corona-radius: $celestial-body-diameter / 2;
// Radius of the corona spread.
$corona-spread-radius: $celestial-body-diameter / 8;

// Normal color of the sky.
$sky-color: #82b1ff;
// Sky color during eclipse.
$eclipsed-sky-color: #04101c;

// How far along through animation the total eclipse occurs.
// Adjusting the gap determines length of total eclipse 
// relative to full animation duration.
$total-eclipse-start-percent: 35%;
$total-eclipse-end-percent: 65%;

// === END CONFIGURATION ===
```

The comments should generally explain what each of these do, but the most important settings are:

- `$duration` - How long the full animation takes.
- `$celestial-body-diameter` - The size of the two celestial bodies relative to the page.
- `$XYZ-colors` - The various colors used throughout the animation.  For example, while the corona color is typically a silvery-white color, feel free to mess around with these settings and see what you can come up with!

### Coloring the Sky

The first thing we do is alter the background of the page, which is effectively the color of the sky, through the `body` tag:

```scss
body {
	animation: adjust-brightness $duration linear infinite;
}
```

If you aren't familiar with it, we're making heavy use of the [`animation`](https://developer.mozilla.org/en-US/docs/Web/CSS/animation) property, which allows us to animate transitions from one CSS configuration to another.  The properties we provide start with `adjust-brightness`, which is actually the `animation-name` property of an animation `@keyframes` configuration we specify elsewhere:

```scss
@keyframes adjust-brightness {
	0%, 100% {
		background-color: $sky-color;
	}
	#{$total-eclipse-start-percent}, #{$total-eclipse-end-percent} {
		background-color: $eclipsed-sky-color;
	}
}
```

CSS [`@keyframes`](https://developer.mozilla.org/en-US/docs/Web/CSS/@keyframes) are basically just like normal animation keyframes, allowing us to specify different properties and CSS configurations at various steps in the animation sequence.  Throughout this project we'll use percentage keyframes, either directly or via `Sass` `$variables`.  For example, the `adjust-brightness` animation above sets keyframes at `0%` and `100%`, which indicate that the `background-color` property should be the normal `$sky-color`.  This means our sky will be its default color at both the beginning and the end of the full animation.

From there, we also have two more keyframes set to the values of `$total-eclipse-start-percent` and `$total-eclipse-end-percent`, which specify moments in the animation sequence that the `background-color` should be changed to `$eclipsed-sky-color`.  Since these are keyframes, the CSS engine will automatically transform between these two configuration states over time, based on the percentages of the total animation time we've specified above.

Looking back up a bit at the `body:animation` property, we also indicate that the duration of the animation should be the `$duration` of our full animation.  We also want the animation to be `linear` (meaning it won't ease in or out but will perform static transition speeds), and should repeat forever.

### And On This Day, We Created The Sun and The Moon

To give our empty `div` elements some actual dimensions, we merely need to add a few properties to the `.eclipse` class:

```scss
.eclipse {
	position: absolute;
	top: 0; right: 0; bottom: 0; left: 0;
	margin: auto;
	width: $celestial-body-diameter;
	height: $celestial-body-diameter;
	border-radius: 50%;
	// ...
}
```

Here we're making sure the sun and moon are centered, and also give them a size of `$celestial-body-diameter`, so we can easily adjust how big they are in the configuration.

### Moving the Moon

Now let's move the moon!  We start by using the ampersand (`&`) shorthand syntax that `Sass` provides to call the parent selector so we can easily create a _nested_ selector for the `.moon` class inside the `.eclipse` class:

```scss
.eclipse {
	// ...
	// &.moon is equivalent to .eclipse.moon.
	&.moon {
		z-index: 1;
		animation: eclipse $duration linear infinite;
	}
}
```

As indicated by the comment, `&` is basically just a shorthand way of specifying the parent selector, `.eclipse` in this case.

We've adjusted the `z-index` property so the moon appears on top of the sun element.  Once again, we also use the same `animation` property settings that we saw before, except this time we're specifying a different `animation-name` of `eclipse`, which can be found below:

```scss
@keyframes eclipse {
	0% { 
		// Offset moon starting position so it fades in.
		left: $celestial-body-diameter * 2.5;
		background-color: $moon-color;
	}
	#{$total-eclipse-start-percent}, #{$total-eclipse-end-percent} {
		left: 0;
		background-color: $eclipsed-sky-color;
	}
	100% { 
		// Offset moon ending position so it fades out.
		left: -$celestial-body-diameter * 2.5;
		background-color: $moon-color;
	}
}
```

The goal here is to move the moon element from right to left, pausing briefly while it's centered over top of the sun element to simulate that brief period of total eclipse that's oh so cool.  To achieve this, we'll use some percentage keyframes, again.  `0%` indicates the moon should be offset to the right about 250% of the distance of the sun (and moon) itself, so the animation doesn't start with the moon already overlapping the sun.  We also set the moon to its own `$moon-color` -- something just slightly off-white by default.

Again, we use the same eclipse percentage offsets as before to indicate when, in the animation sequence, the moon should come to a halt in front of the sun.  It should also change its color to match the `$eclipsed-sky-color`, since it's completely in shadow at that point and, thus, would appear as a very similar color.

Finally, for the `100%` keyframe we apply a negative offset to keep the moon moving to the left and adjust its color back to normal.

### Lemme See Your Corona

Finally, we want to simulate a corona effect (the white halo that appears around the sun when a total eclipse occurs).  Even though the sun element is stationary throughout the entire animation, to create a corona we need to apply an `animation` property to the `.sun` element:

```scss
.eclipse {
	// ...
	&.sun {
		z-index: 0;
		background-color: $sun-color;
		animation: corona $duration linear infinite;
	}
}
```

The `corona` `animation-name` is, once again, a percentage keyframe configuration.  Here we're using the [`box-shadow`](https://developer.mozilla.org/en-US/docs/Web/CSS/box-shadow) property to create a full-circumference shadow effect all around the sun element.  We can apply a bit of blur and spreading to try to get just the right look.  While this won't be perfect (the actual corona is must less uniform, for example), it'll get the job done for a simple CSS animation!

```scss
@keyframes corona {
	0%, 100% {
		box-shadow: unset;
	}
	// Create a slight corona effect during eclipse.
	#{$total-eclipse-start-percent}, #{$total-eclipse-end-percent} {
		box-shadow: 0px 0px $corona-radius $corona-spread-radius $corona-color;
	}
}
```

We start by applying the `unset` value at the start and end of the animation.  Then, during the same stop and start keyframes that indicate when the moon is paused in front of the sun, we create a shadow without size, but only `radius` and `spread`, instead.  This causes that blurring and outward-spreading effect that we're after.  We've also set the color of the corona to `$corona-color`, since the corona is much lighter than the normal color of the sun.

---

And that's it!  We've created the sun and the moon, moved the moon, adjusted the sky color, and added a corona during the total eclipse.  Loading up the `index.html` page in your browser should show you a nice little sun with a moon slowly eclipsing it, before getting bored of that job and moseying along.

---

__META DESCRIPTION__

To celebrate the Solar Eclipse of 2017, we have a detailed tutorial showing how to create a fun solar eclipse using a bit of some Sass-powered CSS!

---

__SOURCES__

- https://codepen.io/search/pens?q=eclipse&limit=all&type=type-pens