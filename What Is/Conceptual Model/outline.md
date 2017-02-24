# Conceptual Model - What is it and how do you use it? [OUTLINE - v1]

- Introduce the term of `conceptual model` (`CM`), as a representation of a system using concepts (as opposed to physical models).
- Emphasize that `CMs` are often used as abstractions of real world objects.
- Distinguish between models of a concept and models that are conceptual.
- Explain the purpose of the article: to explore what `CMs` are, how they are implemented, and advantages and disadvantages of using `CM` throughout the development life cycle.

## What is a Conceptual Model?

- Discuss the primary purpose of conceptual modelling, as a way to describe physical/social aspects of the world in an abstract way.
- Highlight that `CM` is often also referred to as `domain model`.
- Detail the fundamental objectives of `CM`:
  - Enhance an individual's understanding of the representative system
  - Facilitate efficient conveyance of system details between stakeholders
  - Provide a point of reference for system designers to extract system specifications
  - Document the system for future reference and provide a means for collaboration
- Outline how the `CM` fits into typical software development life cycles.

> A conceptual data model captures the key business entities (a person, place, concept, event or thing about which the organization wants to collect data), and the relationships between these entities.

> - https://en.wikipedia.org/wiki/Conceptual_model
> - https://en.wikipedia.org/wiki/Domain_model

## Conceptual Model Implementation and Best Practices

- Discuss a few common implementations of `CMs`:
  - Data Modeling
  - Rapid Application Development
  - Unified Modeling Language
  - etc
- Give example of common best practices:
  - `CM` available to all team members
  - Easily changeable, reflecting up-to-date information
  - Written and visual diagramming
  - Establish relevant terms and concepts
  - Define each term/concept as an entity for use in `CM`
  - Begin structuring entities within `CM`
  - Diagram or explain relationships between all entities
- Provide examples of real-world implementations of `CM` (MDA, FEMA, NIST, etc).

> - https://en.wikipedia.org/wiki/Conceptual_model_(computer_science)
> - https://en.wikipedia.org/wiki/Conceptual_schema
> - https://en.wikipedia.org/wiki/Data_flow_diagram
> - https://en.wikipedia.org/wiki/Data_modeling#Data_models
> - https://en.wikipedia.org/wiki/Rapid_application_development
> - https://en.wikipedia.org/wiki/Unified_Modeling_Language

> - https://www.acm-sigsim-mskr.org/Courseware/Balci/Slides/BalciSlides-08-ConceptualModeling.pdf
> - http://searchdatamanagement.techtarget.com/feature/A-guide-to-conceptual-data-models-for-IT-managers
> - http://searchcrm.techtarget.com/definition/entity-relationship-diagram

## Advantages of Conceptual Modeling

- Discuss primary advantages to `CM` implementation:
  - Helps establish all entities and relationships which are likely to be necessary in software development life cycle.
  - Defines scope of project and assists with scheduling.
  - First step prior to producing other, less abstract models.
  - `CM` can be used as basis for other models like `logical data model`.
  - Great for high-level understanding of system, particularly for managers and executives not dealing with coding/implementation.

> - http://serc.carleton.edu/introgeo/conceptmodels/why.html
> - http://searchdatamanagement.techtarget.com/answer/What-are-the-benefits-of-a-conceptual-data-model

## Disadvantages of Conceptual Modeling

- Discuss disadvantages of `CM` implementation:
  - While `CM` can (and should) be adaptive, creation requires a rather fundamental and robust understanding of project and all associated entities/relationships.
  - Improper entity/relationship modelling could lead to massive time wasting/sunk costs.
  - Potential for clashes between various components of project (I/O, requirements, design, code, deployment, etc)
  - Difficulty implementing `CM` for particularly complex applications.
  
> - https://pdfs.semanticscholar.org/b84d/1261de283fe754ade0f5540c1c2ec530a39b.pdf
> - https://link.springer.com/chapter/10.1007%2F978-3-540-72677-7_4
> - http://www.conceptualmodeling.org/ConceptualModeling.html