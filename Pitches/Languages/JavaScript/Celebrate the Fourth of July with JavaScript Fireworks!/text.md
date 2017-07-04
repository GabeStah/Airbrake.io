# Celebrate the Fourth of July with JavaScript Fireworks!

Many Americans celebrate the Fourth of July by hanging out with friends, having a backyard barbeque while enjoying some cool drinks and watching a wonderful display of colorful fireworks.  In honor of the holiday, we thought we'd create our own little fireworks display through code.

Even if you aren't from America yourself, fireworks can still be fun and beautiful, so come celebrate with us and learn how to create your own cool fireworks display with a bit of JavaScript!

## Tools You'll Need 

To begin this project all you need is a modern web browser and whatever text editor you prefer.  I'm using Google Chrome and Visual Studio Code, but most anything you are comfortable with will work just fine; even Notepad!

## Project Structure

We'll be using JavaScript for this tutorial because its simplicity and its prominence -- just about every computer or device out there can handle JavaScript, so there shouldn't be any compatibility issues for those of you following along.

Our entire application will consist of just three files:

- `index.html`: This is the HTML page we'll load in our browser to view our finished work.
- `default.css`: The cascading stylesheet that contains the (minor) styling adjustments we'll need to make things look great.
- `app.js`: The meat and potatoes of our application.  This is where all our firework-generating goodness will reside.

That's all we'll need, so start by adding a new directory on your computer and creating those three blank files.  Alternatively, you can use an online development playground tool like [`Codepen`](https://codepen.io/pen/), in which case everything will appear in the browser as you follow along.

## The End Goal

Since this is an animated fireworks display that we're going for, it's rather difficult to portray what the final result will look like in writing.  If you want to see what the final product will look like (as well as view the full code all in one place), have a look at [this CodePen entry](https://codepen.io/GabeStah/pen/BZxJmy).

## Creating the HTML Page

Let's begin by writing the simple HTML that we'll need to display our fireworks application.  We're going to be using the powerful [`canvas API`](https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API) that was introduced with `HTML5`, which makes it very easy to create all sorts of web-based graphics using real-time languages like JavaScript.

Therefore, most of our drawing and creation logic takes place in the `app.js` file, so the contents of `index.html` is fairly sparse.  Copy and paste the following into `index.html`:

```html
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8" />
        <title>Fourth of July Fireworks!</title>
        <link href="default.css" rel="stylesheet">
        <script>
            // Load app.js script after document has rendered.
            var script = document.createElement('script');
            script.src = 'app.js';
            script.type = 'text/javascript';
            document.getElementsByTagName('head')[0].appendChild(script);
        </script>
    </head>
    <body>
        <canvas id="canvas">Your browser does not support the 'canvas' element.</canvas>
    </body>
</html>
```

Our `index.html` file begins with a standard `DOCTYPE` specification, along with the `<meta>` tag where we specify the character set we're using, along with the page `<title>`.  We also need to include our `default.css` stylesheet, so the `<link>` tag comes next to accomplish that.

Next is our (limited) `<script>` block.  Normally we'd directly include the `app.js` script file via `<script src="app.js"></script>`.  However, it's important that we attempt to load the `app.js` code _after_ our page has fully loaded.  Since we're not using any outside libraries like jQuery, which would normally give us the means to do this automatically, we need a few lines inside the `<script>` tag to force our HTML page to load `app.js` after the page has finished loading all other elements.

The final thing we need to add is the `<canvas>` element, which is where all the magic happens and where our fireworks graphics will actually be drawn.

## Creating the Stylesheet

Our style content for this tutorial is extremely simple.  Start by opening the `default.css` file that was created earlier and paste the following into it:

```css
/* Create a dark background. */
body {
    background: #000;
    margin: 0;
}

/* Fit the canvas to the page width. */
canvas {
    display: block;
}
```

As indicated by the comments, all we're doing here is changing the `<body>` background to black and removing the `margins`.  For the critical `<canvas>` element we're simply changing the `display` style to `block`, which ensures that our canvas stretches the entire width of the page.

That's all we need to do for styling, since most of the graphics will be created in real-time through our JavaScript code.  It's also important to note here that we're using so little CSS that we could've just included the inline styling directly inside our `index.html` page using `<style></style>` tags.  However, I'd generally recommend avoiding inline styling, since it helps to keep projects much cleaner to separate styling (`CSS`) from layout (`HTML`) whenever possible.

## Writing Our JavaScript Code

Finally we get to the fun part -- writing some JavaScript code!  There are many ways to accomplish the goal of creating fireworks (just browse through the plethora of [examples on CodePen](https://codepen.io/search/pens?q=fireworks&limit=all&type=type-pens) and elsewhere), but for this script we've opted for a fairly simple approach.  Our fireworks display will be made up of two types of elements: `fireworks` and `particles`.

A `firework` (within our code) represents a single trail that is launched from the bottom of the screen to a destination position.  Once it reaches its destination, it should explode and create a series of `particles`, which will shimmer and rain down, just like real fireworks!

### Configuration

To get started simply open up (or create) the `app.js` file.  One goal of this example is to allow for a lot of customization, so you can easily modify the behavior of nearly all elements of the fireworks to see what cool things you can come up with.  Therefore, we begin our `app.js` file with all the `configuration` options:

```js
// === CONFIGURATION ===

// Base firework acceleration.
// 1.0 causes fireworks to travel at a constant speed.
// Higher number increases rate firework accelerates over time.
const FIREWORK_ACCELERATION = 1.05;
// Minimum firework brightness.
const FIREWORK_BRIGHTNESS_MIN = 50;
// Maximum firework brightness.
const FIREWORK_BRIGHTNESS_MAX = 70;
// Base speed of fireworks.
const FIREWORK_SPEED = 5;
// Base length of firework trails.
const FIREWORK_TRAIL_LENGTH = 3;
// Determine if target position indicator is enabled.
const FIREWORK_TARGET_INDICATOR_ENABLED = true;

// Minimum particle brightness.
const PARTICLE_BRIGHTNESS_MIN = 50;
// Maximum particle brightness.
const PARTICLE_BRIGHTNESS_MAX = 80;
// Base particle count per firework.
const PARTICLE_COUNT = 80;
// Minimum particle decay rate.
const PARTICLE_DECAY_MIN = 0.015;
// Maximum particle decay rate.
const PARTICLE_DECAY_MAX = 0.03;
// Base particle friction.
// Slows the speed of particles over time.
const PARTICLE_FRICTION = 0.95;
// Base particle gravity.
// How quickly particles move toward a downward trajectory.
const PARTICLE_GRAVITY = 0.7;
// Variance in particle coloration.
const PARTICLE_HUE_VARIANCE = 20;
// Base particle transparency.
const PARTICLE_TRANSPARENCY = 1;
// Minimum particle speed.
const PARTICLE_SPEED_MIN = 1;
// Maximum particle speed.
const PARTICLE_SPEED_MAX = 10;
// Base length of explosion particle trails.
const PARTICLE_TRAIL_LENGTH = 5;

// Alpha level that canvas cleanup iteration removes existing trails.
// Lower value increases trail duration.
const CANVAS_CLEANUP_ALPHA = 0.3;
// Hue change per loop, used to rotate through different firework colors.
const HUE_STEP_INCREASE = 0.5;

// Minimum number of ticks per manual firework launch.
const TICKS_PER_FIREWORK_MIN = 5;
// Minimum number of ticks between each automatic firework launch.
const TICKS_PER_FIREWORK_AUTOMATED_MIN = 20;
// Maximum number of ticks between each automatic firework launch.
const TICKS_PER_FIREWORK_AUTOMATED_MAX = 80;

// === END CONFIGURATION ===
```

We've purposefully opted to use the `const` keyword for all the configuration settings.  This is generally good practice since it ensures we cannot accidentally change the value during runtime.  Most of the configuration options are reasonably commented and given appropriate default values, but once our application is up and running, feel free to play with these settings and see what you can do!

Our configuration is split into four main categories: Fireworks settings to adjust the behavior of the fireworks themselves, particles to adjust the behavior of explosion particles, canvas coloration to modify global behavior, and tick restrictions to adjust how frequently fireworks are launched.

### Local Variables

The next section houses all our local variables.  These are _not_ constants, and will be used and modified frequently throughout the entire project.  Go ahead and add this next section to `app.js`:

```js
// === LOCAL VARS ===

let canvas = document.getElementById('canvas');
// Set canvas dimensions.
canvas.width = window.innerWidth;
canvas.height = window.innerHeight;
// Set the context, 2d in this case.
let context = canvas.getContext('2d');
// Firework and particles collections.
let fireworks = [], particles = [];
// Mouse coordinates.
let mouseX, mouseY;
// Variable to check if mouse is down.
let isMouseDown = false;
// Initial hue.
let hue = 120;
// Track number of ticks since automated firework.
let ticksSinceFireworkAutomated = 0;
// Track number of ticks since manual firework.
let ticksSinceFirework = 0;

// === END LOCAL VARS ===
```

Again, most variables are commented, but critical ones include the `fireworks = []` and `particles = []` arrays that we'll use to keep track all the individual instances of fireworks and particles we have active at any given time.

### Helper Functions

While not required, it's often beneficial to create a few helper functions in any application.  These should be **independent** functions that are not tied into the logic or functionality of your application in anyway.  Think of them like a library that can be used in any project.

For our purposes we just have a few helper functions to add to `app.js`:

```js
// === HELPERS ===

// Use requestAnimationFrame to maintain smooth animation loops.
// Fall back on setTimeout() if browser support isn't available.
window.requestAnimFrame = (() => {
	return 	window.requestAnimationFrame ||
		   	window.webkitRequestAnimationFrame ||
		   	window.mozRequestAnimationFrame ||
		   	function(callback) {
		   		window.setTimeout(callback, 1000 / 60);
			};
})();

// Get a random number within the specified range.
function random(min, max) {
	return Math.random() * (max - min) + min;
}

// Calculate the distance between two points.
function calculateDistance(aX, aY, bX, bY) {
	let xDistance = aX - bX;
	let yDistance = aY - bY;
	return Math.sqrt(Math.pow(xDistance, 2) + Math.pow(yDistance, 2));
}

// === END HELPERS ===
```

In addition to `random()` to get a random value within a random and `calculateDistance()` to figure out how far apart two points are, a very critical helper is `window.requestAnimFrame()`.  This immediately invoked function expression (`IIFE`) we've defined here allows us to call the appropriate [`requestAnimationFrame()`](https://developer.mozilla.org/en-US/docs/Web/API/window/requestAnimationFrame) method of whatever browser is being used.  `requestAnimationFrame()` is a smooth way to request the browser redraw the window, without resorting to weaker techniques like using `setTimeout()` (which should be a last resort for modern development).

### Event Listeners

One small (but fun) feature we'll be adding is the ability to click anywhere on the page to manually launch a new firework toward that specific position.  To accomplish this we need to know where the mouse position is at any given moment, while also tracking when the mouse button is pressed and released.  These event listener functions allow us to update relevant variables when the mouse is moved or clicked:

```js
// === EVENT LISTENERS ===

// Track current mouse position within canvas.
canvas.addEventListener('mousemove', (e) => {
	mouseX = e.pageX - canvas.offsetLeft
	mouseY = e.pageY - canvas.offsetTop
});

// Track when mouse is pressed.
canvas.addEventListener('mousedown', (e) => {
	e.preventDefault()
	isMouseDown = true
});

// Track when mouse is released.
canvas.addEventListener('mouseup', (e) => {
	e.preventDefault()
	isMouseDown = false
});

// === END EVENT LISTENERS ===
```

### Prototyping

The prototyping section of our `app.js` code is where the majority of our logic takes place.  This is where we'll define how fireworks and particles are created, updated, and eventually destroyed.  We'll use the great number of configuration settings we defined at the top of the `app.js` file, along with elapsed time (in the form of `ticks` or loops of our animation), to determine what should be changed.  This will be a fairly extensive section, so we'll break it up into individual function definitions to make it easier to digest.

As the name of this section indicates, we'll be using the tried-and-true [`prototyping`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/prototype) technique to create our critical `Firework` and `Particle` objects.  This allows us to track individual instances of these elements easily, while also assigning each instance a unique set of properties that will affect its behavior.

#### Handling Fireworks

We begin with the `Firework` object:

```js
// === PROTOTYPING ===

// Creates a new firework.
// Path begins at 'start' point and ends and 'end' point.
function Firework(startX, startY, endX, endY) {
	// Set current coordinates.
	this.x = startX;
	this.y = startY;
	// Set starting coordinates.
	this.startX = startX;
	this.startY = startY;
	// Set end coordinates.
	this.endX = endX;
	this.endY = endY;
	// Get the distance to the end point.
	this.distanceToEnd = calculateDistance(startX, startY, endX, endY);
	this.distanceTraveled = 0;
	// Create an array to track current trail particles.
	this.trail = [];
	// Trail length determines how many trailing particles are active at once.
	this.trailLength = FIREWORK_TRAIL_LENGTH;
	// While the trail length remains, add current point to trail list.
	while(this.trailLength--) {
		this.trail.push([this.x, this.y]);
	}
	// Calculate the angle to travel from start to end point.
	this.angle = Math.atan2(endY - startY, endX - startX);
	// Set the speed.
	this.speed = FIREWORK_SPEED;
	// Set the acceleration.
	this.acceleration = FIREWORK_ACCELERATION;
	// Set the brightness.
	this.brightness = random(FIREWORK_BRIGHTNESS_MIN, FIREWORK_BRIGHTNESS_MAX);
	// Set the radius of click-target location.
	this.targetRadius = 2.5;
}
```

Most of this function is used to set the assortment of properties that our `Firework` will use elsewhere.  Of particular note is the start and end point parameter coordinates that tell the `Firework` the path it should travel on.  We use these points to calculate the distance it will travel, the angle of trajectory, and so forth.

Next is the `Firework.prototype.update()` method:

```js
// Update a firework prototype.
// 'index' parameter is index in 'fireworks' array to remove, if journey is complete.
Firework.prototype.update = function(index) {
	// Remove the oldest trail particle.
	this.trail.pop();
	// Add the current position to the start of trail.
	this.trail.unshift([this.x, this.y]);
	
	// Animate the target radius indicator.
	if (FIREWORK_TARGET_INDICATOR_ENABLED) {
		if(this.targetRadius < 8) {
			this.targetRadius += 0.3;
		} else {
			this.targetRadius = 1;
		}
	}
	
	// Increase speed based on acceleration rate.
	this.speed *= this.acceleration;
	
	// Calculate current velocity for both x and y axes.
	let xVelocity = Math.cos(this.angle) * this.speed;
	let yVelocity = Math.sin(this.angle) * this.speed;
	// Calculate the current distance travelled based on starting position, current position, and velocity.
	// This can be used to determine if firework has reached final position.
	this.distanceTraveled = calculateDistance(this.startX, this.startY, this.x + xVelocity, this.y + yVelocity);
	
	// Check if final position has been reached (or exceeded).
	if(this.distanceTraveled >= this.distanceToEnd) {
		// Destroy firework by removing it from collection.
		fireworks.splice(index, 1);
		// Create particle explosion at end point.  Important not to use this.x and this.y, 
		// since that position is always one animation loop behind.
		createParticles(this.endX, this.endY);		
	} else {
		// End position hasn't been reached, so continue along current trajectory by updating current coordinates.
		this.x += xVelocity;
		this.y += yVelocity;
	}
}
```

This is where most of the behavioral logic of our `Firework` takes place.  We start by adjusting the `trail` property array, which we're using to track a list of active positions that should be used to draw our firework trail graphic.

We also adjust the `speed` and use the calculated `velocity` to determine how far this firework has travelled.  We use this travelled distance to determine if the firework has reached its target destination, in which case we destroy it and create a new explosion of particles via `createParticles()`.

The final `Firework` prototype is the `draw()` method:

```js
// Draw a firework.
// Use CanvasRenderingContext2D methods to create strokes as firework paths. 
Firework.prototype.draw = function() {
	// Begin a new path for firework trail.
	context.beginPath();
	// Get the coordinates for the oldest trail position.	
	let trailEndX = this.trail[this.trail.length - 1][0];
	let trailEndY = this.trail[this.trail.length - 1][1];
	// Create a trail stroke from trail end position to current firework position.
	context.moveTo(trailEndX, trailEndY);
	context.lineTo(this.x, this.y);
	// Set stroke coloration and style.
	// Use hue, saturation, and light values instead of RGB.
	context.strokeStyle = `hsl(${hue}, 100%, ${this.brightness}%)`;
	// Draw stroke.
	context.stroke();
	
	if (FIREWORK_TARGET_INDICATOR_ENABLED) {
		// Begin a new path for end position animation.
		context.beginPath();
		// Create an pulsing circle at the end point with targetRadius.
		context.arc(this.endX, this.endY, this.targetRadius, 0, Math.PI * 2);
		// Draw stroke.
		context.stroke();
	}
}
```

This is where all visual updates are made to this particular `Firework` instance.  All we're really doing is creating a new `line` that spans between the current position of the firework and the tail end of the `trail`.

We also have a second stroke using the `arc()` method that creates our target circle indicator, showing the position that the firework is heading toward.

#### Handling Particles

The next section contains the latter half of our prototyping code, which we're using to handle our `Particle` elements.  As we've already seen, `Particles` are the series of trails that are generated when a `Firework` reaches its target position and explodes.

Since much of the `Particle` prototype code is similar to that of `Firework` we don't need to explain it in greater detail outside of the comments, so feel free to add the following to `app.js` to continue:

```js
// Creates a new particle at provided 'x' and 'y' coordinates.
function Particle(x, y) {
	// Set current position.
	this.x = x;
	this.y = y;
	// To better simulate a firework, set the angle of travel to random value in any direction.
	this.angle = random(0, Math.PI * 2);
	// Set friction.
	this.friction = PARTICLE_FRICTION;
	// Set gravity.
	this.gravity = PARTICLE_GRAVITY;
	// Set the hue to somewhat randomized number.
	// This gives the particles within a firework explosion an appealing variance.
	this.hue = random(hue - PARTICLE_HUE_VARIANCE, hue + PARTICLE_HUE_VARIANCE);
	// Set brightness.
	this.brightness = random(PARTICLE_BRIGHTNESS_MIN, PARTICLE_BRIGHTNESS_MAX);
	// Set decay.
	this.decay = random(PARTICLE_DECAY_MIN, PARTICLE_DECAY_MAX);	
	// Set speed.
	this.speed = random(PARTICLE_SPEED_MIN, PARTICLE_SPEED_MAX);
	// Create an array to track current trail particles.
	this.trail = [];
	// Trail length determines how many trailing particles are active at once.
	this.trailLength = PARTICLE_TRAIL_LENGTH;
	// While the trail length remains, add current point to trail list.
	while(this.trailLength--) {
		this.trail.push([this.x, this.y]);
	}
	// Set transparency.
	this.transparency = PARTICLE_TRANSPARENCY;
}

// Update a particle prototype.
// 'index' parameter is index in 'particles' array to remove, if journey is complete.
Particle.prototype.update = function(index) {
	// Remove the oldest trail particle.
	this.trail.pop();
	// Add the current position to the start of trail.
	this.trail.unshift([this.x, this.y]);

	// Decrease speed based on friction rate.
	this.speed *= this.friction;
	// Calculate current position based on angle, speed, and gravity (for y-axis only).
	this.x += Math.cos(this.angle) * this.speed;
	this.y += Math.sin(this.angle) * this.speed + this.gravity;

	// Apply transparency based on decay.
	this.transparency -= this.decay;
	// Use decay rate to determine if particle should be destroyed.
	if(this.transparency <= this.decay) {
		// Destroy particle once transparency level is below decay.
		particles.splice(index, 1);
	}
}

// Draw a particle.
// Use CanvasRenderingContext2D methods to create strokes as particle paths. 
Particle.prototype.draw = function() {
	// Begin a new path for particle trail.
	context.beginPath();
	// Get the coordinates for the oldest trail position.	
	let trailEndX = this.trail[this.trail.length - 1][0];
	let trailEndY = this.trail[this.trail.length - 1][1];
	// Create a trail stroke from trail end position to current particle position.
	context.moveTo(trailEndX, trailEndY);
	context.lineTo(this.x, this.y);
	// Set stroke coloration and style.
	// Use hue, brightness, and transparency instead of RGBA.
	context.strokeStyle = `hsla(${this.hue}, 100%, ${this.brightness}%, ${this.transparency})`;
	context.stroke();
}

// === END PROTOTYPING ===
```

### Application Helper Functions

Next we need to add a series of helper functions that are _specifically_ tied to the logic and functionality of our application.  That means they make direct use of the various app-specific variables and objects we've defined, so these functions are distinctly different from the `helper functions` we defined earlier.

We start with `cleanCanvas()`, which you can read more about in the source comments, but is primarily meant to cleanup existing trails based on their current transparency/alpha level:

```js
// Cleans up the canvas by removing older trails.
// ...
function cleanCanvas() {
	// Set 'destination-out' composite mode, so additional fill doesn't remove non-overlapping content.
	context.globalCompositeOperation = 'destination-out';
	// Set alpha level of content to remove.
	// Lower value means trails remain on screen longer.
	context.fillStyle = `rgba(0, 0, 0, ${CANVAS_CLEANUP_ALPHA})`;
	// Fill entire canvas.
	context.fillRect(0, 0, canvas.width, canvas.height);
	// Reset composite mode to 'lighter', so overlapping particles brighten each other.
	context.globalCompositeOperation = 'lighter';
}
```

We've called `createParticles()` elsewhere already, and we're using this method to create a series of new `Particle` instances when a `Firework` explodes:

```js
// Create particle explosion at 'x' and 'y' coordinates.
function createParticles(x, y) {
	// Set particle count.
	// Higher numbers may reduce performance.
	let particleCount = PARTICLE_COUNT;
	while(particleCount--) {
		// Create a new particle and add it to particles collection.
		particles.push(new Particle(x, y));
	}
}
```

`launchAutomatedFirework()` is executed every animation loop and determines if enough time has passed to launch another automatic firework.  We launch fireworks from the bottom center of the screen and target them toward a random position in the upper half:

```js
// Launch fireworks automatically.
function launchAutomatedFirework() {
	// Determine if ticks since last automated launch is greater than random min/max values.
	if(ticksSinceFireworkAutomated >= random(TICKS_PER_FIREWORK_AUTOMATED_MIN, TICKS_PER_FIREWORK_AUTOMATED_MAX)) {
		// Check if mouse is not currently clicked.
		if(!isMouseDown) {
			// Set start position to bottom center.
			let startX = canvas.width / 2;
			let startY = canvas.height;
			// Set end position to random position, somewhere in the top half of screen.
			let endX = random(0, canvas.width);
			let endY = random(0, canvas.height / 2);
			// Create new firework and add to collection.
			fireworks.push(new Firework(startX, startY, endX, endY));
			// Reset tick counter.
			ticksSinceFireworkAutomated = 0;
		}
	} else {
		// Increment counter.
		ticksSinceFireworkAutomated++;
	}
}
```

`launchManualFirework()` is also executed every animation frame and checks if enough time has elapsed to launch a _manual_ firework, assuming the mouse button is currently pressed:

```js
// Launch fireworks manually, if mouse is pressed.
function launchManualFirework() {
	// Check if ticks since last firework launch is less than minimum value.
	if(ticksSinceFirework >= TICKS_PER_FIREWORK_MIN) {
		// Check if mouse is down.
		if(isMouseDown) {
			// Set start position to bottom center.
			let startX = canvas.width / 2;
			let startY = canvas.height;
			// Set end position to current mouse position.
			let endX = mouseX;
			let endY = mouseY;
			// Create new firework and add to collection.
			fireworks.push(new Firework(startX, startY, endX, endY));
			// Reset tick counter.
			ticksSinceFirework = 0;
		}
	} else {
		// Increment counter.
		ticksSinceFirework++;
	}
}
```

Our last two app helper functions are `updateFireworks()` and `updateParticles()`, which perform their respective tasks of drawing and updating the status of all existing `Firework/Particle` elements that are currently active:

```js
// Update all active fireworks.
function updateFireworks() {
	// Loop backwards through all fireworks, drawing and updating each.
	for (let i = fireworks.length - 1; i >= 0; --i) {
		fireworks[i].draw();
		fireworks[i].update(i);
	}
}

// Update all active particles.
function updateParticles() {
	// Loop backwards through all particles, drawing and updating each.
	for (let i = particles.length - 1; i >= 0; --i) {
		particles[i].draw();
		particles[i].update(i);
	}
}

// === END APP HELPERS ===
```

Last but certainly not least comes the main `loop()` function, which drives the entire application by recursively passing itself to each `requestAnimFrame()` call.  This ensure that each frame all our `Fireworks` and `Particles` are moved, updated, and redrawn as necessary, while also creating any new fireworks that need to be launched.  The final snippet of code to add to `app.js` is as follows:

```js
// Primary loop.
function loop() {
	// Smoothly request animation frame for each loop iteration.
	requestAnimFrame(loop);

	// Adjusts coloration of fireworks over time.
	hue += HUE_STEP_INCREASE;

	// Clean the canvas.
	cleanCanvas();

	// Update fireworks.
	updateFireworks();

	// Update particles.
	updateParticles();
	
	// Launch automated fireworks.
	launchAutomatedFirework();
	
	// Launch manual fireworks.
	launchManualFirework();
}

// Initiate loop after window loads.
window.onload = loop;
```

That's all there is to it!  You're now ready to open up the `index.html` file in your browser of choice and watch the fireworks fly.  Feel free to click around (or hold the mouse button) to launch them quickly.  Best of all, start messing with some of the configuration options to really kick things up a notch.  If your system can handle it, I recommend dramatically increasing `PARTICLE_COUNT` and lowering `CANVAS_CLEANUP_ALPHA` to really make things come alive!  Happy Fourth of July!

---

__META DESCRIPTION__

A detailed tutorial illustrating how to create a fun fireworks display using HTML5 canvas and JavaScript to help celebrate the Fourth of July!

---

__SOURCES__

- https://codepen.io/search/pens?q=fireworks&limit=all&type=type-pens
- https://codepen.io/chuongdang/pen/yzpDG