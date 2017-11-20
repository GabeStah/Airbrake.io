# Is Your Security Scaling Up Along with Your Operation?

Throughout an entire software development life cycle, it is all too common for security practices to be neglected in favor of the more tangible, immediate benefits of adding that next cool application feature or landing another large swath of users.  However, as we've witnessed with the many massive data breaches in recent years, scaling up your information security through proper security testing and analysis has never been more crucial to maintain a healthy business and reputable brand.

In this article we'll briefly examine what modern information security is all about, along with a few tips and tricks you can use within your own organization to maintain proper security testing and other best practices.  Let's dig right in!

## The Tenets of Information Security

The primary tenants of information security revolve around six basic concepts: `confidentiality`, `integrity`, `availability`, `authentication`, `authorization`, and `nonrepudiation`.  Below, we'll briefly explore the each concept and see a simple example of how these ideas are used within and impact the notions of proper information security.

### Confidentiality

Think back to grade school, when you told your friend Jennifer a juicy little secret, only to find out hours later that little snitch told someone else!  Ignoring the dastardly social tactics that Jennifer has undertaken, the fact remains that this is an early example of that information (your secret) losing its `confidentiality`.

The same principles apply to online information security.  In most cases, it is often _critical_ that certain types of data remain completely confidential, since we've witnessed first hand what happens when supposedly confidential information gets out.  The recent Equifax breach is just the latest and highest profile example of this trend, so it's vital that organizations make every effort to keep user data confidential and secure.

In the most mission-critical cases, information confidentiality is a matter of legal requirement.  Medical records, banking transactions, debt collections, and so forth are all examples of information that must _legally_ be kept confidential.

### Integrity

When you heard that Jennifer had told your secret to others, you may have started to notice a few minor modifications to the content of that secret.  A name was slightly changed and the details were a bit jumbled, which means that the `integrity` of the original information had been lost.

In DevOps, loss of integrity can occur when data is accessible over an insecure network, or is improperly handled (whether said mishandling was intentional or not).  As you might imagine, data integrity is particularly crucial when even _slight_ alterations to said data could cause catastrophic results, such as financial services or medical prescriptions. 

### Availability

The next day at school it turns out Jennifer is out sick.  Many other kids are anxiously asking around, trying to get the latest scoop on your secret that she had been spreading.  But, try as they might, Jennifer isn't there to give them all the dirt, and _you_ certainly aren't going to spill the beans.  This situation has created a loss of `availability` for your secret information.

This same loss of availability can occur within information security when data is erased or is otherwise inaccessible.  This unavailability _isn't_ to be confused with _intentional denial_ of said information, where information is purposely withheld from certain users based on credentials or the like.  Instead, information availability refers to an _unintended_ loss or inaccessibility of data -- users who _should_ have access to the data are unable to do so.

Service-oriented businesses typically rely heavily on information availability, so they can properly track customer interactions, handle orders, issue shipments, and so forth.

### Authentication and Authorization

When Jennifer returns to school she seems to have spent her time away coming up with a scheme to determine if any given kid asking for the secret information is worthy of bestowing this glorious tidbit upon them.  She asks each kid that comes to her to prove that they're in her class by showing her their very own one-of-a-kind name tag.  Jennifer does this as a form of `authentication`, to prove that the kid in front of her is who they say they are.  With proper authentication taken care of, Jennifer then decides that only a handful of particular kids are actually `authorized` to receive this secret morsel.

Just as Jennifer first needed to authenticate kids to determine which are authorized to proceed, the same security measures should always be taken and used together in application security to determine if a user is who they say they are, and whether that should allow them access to the requested information.  If a user finds a way to fake their authentication credentials, they can gain access to information that should normally be out of their reach.  Likewise, if authorization can be bypassed, it doesn't matter who (or what role) the system authenticates that user to be.

### Non-Repudiation

In your class, your friend Jennifer decides to get systematic with her secret dispersal techniques.  She starts scratching out little "contracts" on perforated notebook pages, in her writing and with her name on them, stating that she has told your secret to the specific kid standing in front of her.  She then requires this kid to sign the contract _before_ any information will be divulged.  At the end of the day as you're going home, you see Jennifer carrying a big old wad of these contracts, tightly folded up and clutched in her fiendish little mitts.

As it turns out, Jennifer has created her own little form of `non-repudation`.  This term simply means that there is some form of proof of a transaction taking place, such that neither party within the transaction can later refute that the transaction took place.  In Jennifer's case, her little signed contracts are a grade school version of this concept, showing that both she and the other kid she told your secret to agreed to this illicit exchange of information.

In information security it is crucial to create some form of non-repudation within your application.  Whether through logs or historical transaction records, it's important that each transaction be explicitly tracked and marked in some way, so neither the sender nor the recipient can claim that they did not send nor receive the transaction in question.

## Security Testing Best Practices

### Frequent Code Reviews

A [`code review`](https://en.wikipedia.org/wiki/Code_review) is simply an examination of source code written by another developer.  When it comes to improvements in security, a thorough code review process should look for any potential vulnerabilities that the code may introduce.  Even seemingly small, accidental issues, like adding `secret key` strings to the public via a code repository push, could cause major confidentiality, authorization, and authentication issues down the road.

### Perform Vulnerability Assessment and Penetration Testings

Another important step when performing security testing is to assess potential vulnerabilities in the application.  These could range from insecure server access to improper injection attack deterrence, but it's critical that everyone on the team be constantly aware of and thinking about potential vulnerabilities during the software development life cycle.  Since every team member has a unique perspective on the application -- and each member will often have dramatically different training and experience -- every voice can and should be able to create vulnerability assessment reports.

These assessments should then be tested via `penetration testing`, which is the actual act of attempting to exploit the proposed vulnerability and see if it still exists, or if a recent change has closed up that avenue of attack.

Since most applications will go through constant changes to both the code and the environment on which they're being executed, it's crucial that these phases are repeated throughout the entire development life cycle -- particularly just prior to and after a major release.

### Setup Firewalls and Other Self-Protecting Measures

While not all applications are written in a language or framework that allows for it, but consider the the possibility of using self-protection measures within the application runtime itself, _or_ just outside it on the network.  A common example is a `firewall`, which monitors incoming traffic and determines if said traffic is safe and should be allowed, or unsafe and should be blocked.

### Check for Common Threats

Many techniques used to gain unauthorized access to a network or application have been detected and well-documented, so it's important that you and your team use the knowledge and experience gathered by other security professionals to lock down your own application as much as possible.  The [Open Web Application Security Project](https://www.owasp.org/index.php/Main_Page) (`OWASP`) is a great non-profit organization dedicated to tracking, documenting, and improving software security.  Their site includes a great deal of useful information, including numerous checklists like the [Web Application Security Testing Cheat Sheet](https://www.owasp.org/index.php/Web_Application_Security_Testing_Cheat_Sheet), which can be used by your security team to test for many of the most common and dangerous loopholes out there today.

### Implement Error Monitoring Software

While the goal of security testing should be to identify and resolve potential vulnerabilities _before_ they are exploited during production or otherwise become a problem, it's not always the case that you'll be able to identify and catch 100% of these possible issues.  Moreover, not all security problems present themselves as a direct attack.  Often, a security loophole is discovered due to an unexpected error from the application, which requires investigation by the development team.  Resolving this error sometimes sheds light on a security problem that should be resolved, so it's crucial that you and your team stay on top of all errors throughout the entirety of the software development life cycle.

One particularly useful tool is error monitoring software, which provides a constant pulse on the health and reliability of your application, whether it's running in development or even out in the wilds of production.  All errors are quickly identified, examined, and are ready to be fixed, without the need for convoluted user-generated reports or expensive, time-consuming quality assurance support.

The best error monitoring services allow you and your team to see the exact nature of every error, including a plethora of detailed metadata.  For example, when an error occurs using <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-security-testing">Airbrake.io's error monitoring software</a>, Airbrake is able to report the error type, error message, parameters, the API method that generated the error, the detailed backtrace, the client machine data, the environment in which the error occurred, the file that caused the error, and much more.

Thus, not only does error monitoring software allow you to track and immediately identify exceptions when they occur, it also provides a substantial safety net, particularly during production releases.  While you'll still want to plan accordingly and establish a sound security testing suite, error monitoring services provide a bit of breathing room by promising to inform you of any unforeseen issues.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-security-testing">Airbrake.io's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams are using `Airbrake` to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A detailed look at the tenets of information security, including a handful of tips and best practices for properly performing security testing.

---

__SOURCES__

- https://en.wikipedia.org/wiki/Security_testing
- https://www.us-cert.gov/security-publications/introduction-information-security
- https://www.3pillarglobal.com/insights/approaches-tools-techniques-for-security-testing