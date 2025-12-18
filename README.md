# Case Study: Design & Integration Tools

Collaborated closely with artists and UX designers to design and develop tools to improve the creative and integration team's productivity and ability to match figma and photoshop designs within the engine.

---

## Relevant Tech Stack
![Unity](https://img.shields.io/badge/Unity-000000?style=for-the-badge&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![JSON](https://img.shields.io/badge/JSON-000000?style=for-the-badge&logo=json&logoColor=white)
![HLSL](https://img.shields.io/badge/HLSL-007ACC?style=for-the-badge)

---
## Tools & Systems to Explore
> Expand each section below for more detail

<details>
  <summary><strong>"No-Code" Data Binder System</strong></summary>
  
  <blockquote>
  <h2>Responsibilities</h2>
  <ul>
    <li>Designed the code architecture for a component-based Data Binder system with a "No-code" requirement during implementation</li>
    <li>Determined and prioritized needed components with focus towards easy scaling</li>
    <li>Built modular, data-binder components for various unity UGUI components</li>
    <li>Built a single point of entry component for binding multiple data binder components to a single JSON payload requiring a single function call by the consumer upon implementation.</li>
    <li>Built a single point of entry component for generating data binder prefabs from JSON array data requiring a single function call by the consumer upon implementation.</li>
  </ul>

  <hr>

  <h2>Challenge</h2>
  <p>
    Creating data-driven graphics generally involves the manual coding of data mapped to various serialized components.
    This makes the creation of graphics a very programmer-first approach and is not very agile.
  </p>
  <p>
    This also creates an ever-increasing code base to maintain as the graphics increase in scope and new graphics are created.
  </p>

  <hr>

  <h2>Solution</h2>
  <p>
    Use an MVVM (Model-View-ViewModel) approach with reusable components that define which JSON fields to bind to UGUI elements. These binders connect to a parent Data Binder, which supplies JSON data from the graphic class. The graphic class interacts only with the parent binder, keeping binding logic abstracted and enabling rapid, code-free modifications once implemented.
  </p>

  <hr>

  <h2>Impact</h2>
  <p>
    Vastly improved turn-around speed and build stability for graphic modifications, additions, and changes to data binding.
  </p>
  <ul>
    <li><strong>Before ➙</strong> days of development time with extra QA required to catch bugs and issues</li>
    <li><strong>After ➙</strong> hours of integration time with next to zero QA required since design and data modifications resulted in zero changes to code</li>
  </ul>

  <hr>

  <h2>Key Learnings</h2>
  <ul>
    <li>Abstracting out data binding logic vastly helps reduce code complexity and improve scalability</li>
    <li>
      “No-Code” scalable systems like this allow for more agile development, reduced QA scope, and significantly
      reduce code maintenance and the risk of introducing new bugs
    </li>
  </ul>

  </blockquote>
</details>

<details>
  <summary><strong>Image Blend Component & Shader</strong></summary>
  
  <blockquote>
  <h2>Responsibilities</h2>
  <ul>
    <li>Extended the Unity Image component to support a custom shader that allows for various standard image blend types found across all creative tools (darken, multiply, color burn, etc).</li>
    <li>Created a custom HLSL UI shader that supports the various image blend mode and maintains all basic image material functionality.</li>
  </ul>

  <hr>

  <h2>Challenge</h2>
  <p>
    Reproducing designs from tools like Figma or Photoshop can be limited by Unity’s lack of standard blend modes. While final blended images can sometimes be imported directly, dynamic layering often requires in‑engine work, leading to discrepancies between the original design and Unity’s output.
  </p>

  <hr>

  <h2>Solution</h2>
  <p>
    Coming soon.
  </p>

  <hr>

  <h2>Impact</h2>
  <p>
    Coming soon.
  </p>
  
  <hr>

  <h2>Key Learnings</h2>
  <p>
    Coming soon.
  </p>

  </blockquote>
</details>

<details>
  <summary><strong>Event-Driven Contrast Ratio Calculator Component</strong></summary>
  
  <blockquote>
  <h2>Responsibilities</h2>
  <ul>
    <li>Coming soon.
  </ul>

  <hr>

  <h2>Challenge</h2>
  <p>
    Coming soon.
  </p>

  <hr>

  <h2>Solution</h2>
  <p>
    Coming soon.
  </p>

  <hr>

  <h2>Impact</h2>
  <p>
    Coming soon.
  </p>
  
  <hr>

  <h2>Key Learnings</h2>
  <p>
    Coming soon.
  </p>

  </blockquote>
</details>

---

## See for yourself
The demo is fully playable. Simply download as zip and extract (or simply clone the repo). Then run inside of Unity to explore the tools and their implementation.<br>
> Unity engine version 6000.3.0f1

### Not a developer? Not a Problem!
Simply download this build and you can see the various tools in action.<br>
[Download Game Build](https://github.com/jglasspilon/UI-Design-Tools-Sample/raw/refs/heads/main/DesignTools-Build.zip)

### Steps:
1. Download zip
2. Extract all files
3. Enter unzipped build folder and run app **DesignTools.exe**
> Hit Alt+F4 to exit the app

---

# At a Glance
### Data Binder Sample In-Action

---

### Blending Images 

---

### Consistent Readable Text Regardless of Background Color


