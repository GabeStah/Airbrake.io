---
categories: [devops]
date: 2018-01-30
published: true
title: "Easing the Pain of Defect Triage"
---

`Triage` is a medical term used to assign degrees of urgency to wounds or illnesses, in order to decide on the order (priority) of treatment across a large number of patients.  `Defect triage` (or `bug triage`) is a term used in modern software development and is the practice of reviewing, discussing, and categorizing software defects based on their negative severity.  Unfortunately, performing defect triage can be a real pain (pun intended), since the process requires intimate knowledge of the entire software and its intended functionality, the capabilities of all relevant systems and components, and the status of all current or incoming defects in the system.  

To help you ease the pain of defect triage, in today's article we'll be exploring exactly what defect triage is, and we'll look at a few useful tips aimed at helping you implement a robust and powerful triage process into your own software development life cycles.  Let's get to it!

## Priority vs Severity

The entire act of performing defect triage revolves around appropriately defining and categorizing defects into relevant `priority` and `severity` levels.  Thus, it is critical that your organization has a clear understanding of what both priority and severity mean in this context.  The goal is to reduce potential ambiguity as much as possible, so that everyone on the team is clearly aware of the differences between each level of priority and severity that your team will be employing.

### Priority

`Priority` is defined as "the fact or condition of being regarded or treated as more important."  Thus, applying this to defect triage is merely the _act_ of treating or regarding one defect as more important than another.  As dozens, if not hundreds, of defects are discovered and processed by you and your team throughout the entire development life cycle of a given project, it should become instinctual for those involved in the quality assurance process to naturally assign a similar priority level to defects of a certain type.  This will come naturally with practice, but can also be supported by well-defined documentation shared across the team, which aims to outline and detail the priority (and severity) levels that should be used.

Perhaps the most important _practical_ result of prioritizing defects is the actual turnaround time necessary to resolve a defect.  As multiple defects are assigned various priority levels, a natural hierarchy of importance is generated, which ensures that those defects higher on the list are fixed before those further down the list.

You and your team can decide on how many priority levels are required, but it is common practice to use three or four different levels:

- `Urgent` - An `urgent` priority indicates that the defect should be resolved immediately, usually taking precedence over every other defect in the system.
- `High` - A `high` priority is for everything that should be just below `urgent` -- that is, if a defect should be prioritized, but is not mission critical to the stability of the software (or to the ability to test said software), a `high` priority will usually suffice.
- `Medium` - `Medium` priority indicates a bug that is not mission critical, but may still impact functionality in some way.
- `Low` - A `low` priority is for everything else, and is usually assigned to defects that are not mission critical, nor that have any negative impact on functionality.  A common example might be a slight user interface issue in which a label or frame is misplaced -- other than the visual look, the bug doesn't effect anything in the software.

### Severity

`Severity` is defined as "the fact or condition of being severe," which is one of those frustrating definitions that essentially uses the original term _within_ the definition.  `Severe` is defined as "(of something bad or undesirable) very great; intense," so we can extrapolate that severity essentially means "the fact or condition of being intensely undesirable."  In the realm of bug triage, most people think of severity as the seriousness or potential negative impact of a defect, if left unresolved.  Measured severity levels are often directly correlated with the client's (or organization's) risk assessment -- that is, the potential negative impact brought about by the given defect.

Just as with priorities, it is up to your organization to decide on the appropriate number of severity levels.  That said, most teams find parity between the number of priority levels and severity levels to be useful and natural, so three or four is often the standard:

- `Critical` - A `critical` defect is one that completely hinders the software in some way, by either halting normal execution, or by preventing normal testing.  
- `Major` - `Major` severity indicates a bug that halts normal execution of a _component_ or _function_ within the overall software, but still allows for most other components or functionality to remain intact.  
- `Moderate` - A `moderate` severity indicates a bug which negatively impacts the software by causing it to exhibit unnatural behavior, but otherwise doesn't halt execution of any core features or components.
- `Minor` - Lastly, `minor` severity is used for all remaining defects.  These bugs should not negatively impact functionality in anyway.

### Categorizing Defects

There are a many possible priority and severity combinations in which a defect can be categorized, so we'll briefly go over a few common examples here to illustrate how you might consider categorizing your own bugs.

- `Urgent` priority + `critical` severity: Defects that prevent normal execution or testing should be given the highest priority and severity categorizations.
- `High` priority + `minor` severity: Since severity is generally the factor in determining how mission critical a defect is, a bug which is experienced by nearly every tester and upon every execution of the application -- yet _doesn't_ severely impact functionality (such as an improper UI label) -- would likely be assigned a `high` priority and a `minor` severity.
- `Low` priority + `major` severity: This categorization might be assigned to a defect that has a high impact on software functionality, but is _only_ experienced in an unlikely/extreme use- or test-case.
- `Low` priority + `minor` severity: Typically, `low` priority and `minor` severity defects are reserved for issues that have no tangible, functional impact, but still fail to meet quality assurance standards.  These often include things like minor cosmetic issues.

## Performing Defect Triage

The act of performing defect triage can be draining and challenging, but ensuring that the process is carried out efficiently and regularly throughout the development life cycle will generate more robust, stable software upon release.  There are a handful of major stages of the process, each of which consists of a number of minor steps that will force your team to evaluate any and all defects in the system, whether new or existing, and ensure every one of them is properly categorized.

### Screening

The first stage in the defect triage process is initial `screening`, which simply asks a handful of questions aimed at determining which incoming defects can be ignored, and which should be investigated further:

- **Is the defect a duplicate?** - If the bug has been previously reported, the new defect can be discarded.  Alternatively -- and often more appropriately -- an internal reference link can be created within whatever bug tracker software your team is using, so future viewers to the bug report will see that it is a duplicate and can easily view the existing bug.
- **Does the defect occur within a supported software version?** - For projects with multiple long-term releases, older versions will often be considered "end-of-life" after a certain date, indicating that support may no longer be provided.  If a defect is found from an unsupported version it can generally be closed and ignored.
- **Is the defect actual, or a result of user error?** - In some situations it may be that a bug report is generated that is not actually the fault of the software, but rather, is the result of user error.  Now, it may be debatable whether the result of that user's error should or should not prompt a change in future versions of the software, but outside of that exception, such defects can typically be ignored.

### Confirmation

The next stage is `confirmation`, in which one or more team members attempt to replicate new defects, or to confirm the functionality of submitted fixes for (or possible regression of) existing bugs.

This stage is also typically when you team will decide whether a particular bug should be recorded and marked for resolution, or whether it should be ignored or put off until a later date.  As such, it is vital that at least one member of the team is assigned as a `facilitator` of the group meetings aimed at tackling defect triage.  This individual should be in charge of conducting such meetings and coordinating discussion between other stakeholders, each of which can provide input into the severity and priority levels that should be assigned to each defect.

### Categorization

`Categorization` is the real meat and potatoes of the defect triage process.  It is within this stage that the team will actually categorize each and every defect into the pre-defined `priority` and `severity` levels.  As previously mentioned, multiple team members _should_ be capable of completely this categorization process on their own, assuming each individual priority and severity level is well-defined and documented.  That said, it often helps to have multiple people discuss defects during this stage, since members of different teams may have varying levels of insight as to how severe or impactful an issue may be.

#### Revision

A sub-stage of `categorization`, the `revision` stage allows existing bugs to be reviewed, removed, or recategorized, depending on the status of the defect.  An obvious example is a defect that has since been fixed in the latest version -- if QA testing has confirmed the fix, it can be marked as such and set to low priority for follow-up regression testing in the future.

### Integration of Error Monitoring Software

Automated defect tracking and reporting tools, like <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-easing-pain-defect-triage">Airbrake</a>, ensure that your team is immediately aware of exceptions the moment they occur.  Airbrake's powerful <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-easing-pain-defect-triage">error monitoring software</a> guarantees that your team won't need to worry about losing track of that rare production defect that slips through the cracks.  Airbrake provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize defect parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-easing-pain-defect-triage">Airbrake's defect monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their defect handling practices!

---

__META DESCRIPTION__

A helpful guide for easing the pain of defect triage in your own software development projects, with common categorization and bug triage practices.

---

__SOURCES__

- https://docs.moodle.org/dev/Bug_triage
- http://www.softwaretestinghelp.com/how-to-set-defect-priority-and-severity-with-defect-triage-process/