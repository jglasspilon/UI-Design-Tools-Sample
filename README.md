# Case Study: Design & Integration Tools

Collaborated closely with artists and UX designers to design and develop tools to improve the creative and integration team's productivity and ability to match figma and photoshop designs within the engine.

---

## Relevant Tech Stack
![Unity](https://img.shields.io/badge/Unity-000000?style=for-the-badge&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![JSON](https://img.shields.io/badge/JSON-000000?style=for-the-badge&logo=json&logoColor=white)
![HLSL](https://img.shields.io/badge/HLSL-007ACC?style=for-the-badge)

---
## Systems & Tools Breakdown
ℹ️ _Click a system below to expand details_

<details>
  <summary><strong>"No-Code Data Binder System"</strong><br>A reusable MVVM-based UI binding framework that eliminated hard-coded UI logic and reduced iteration time from days to hours.</summary>
  
  <blockquote>
  <h3>Responsibilities</h3>
  <ul>
    <li>Designed the code architecture for a component-based Data Binder system with a "No-code" requirement during implementation</li>
    <li>Determined and prioritized needed components with focus towards easy scaling</li>
    <li>Built modular, data-binder components for various unity UGUI components</li>
    <li>Built a single point of entry component for binding multiple data binder components to a single JSON payload requiring a single function call by the consumer upon implementation.</li>
    <li>Built a single point of entry component for generating data binder prefabs from JSON array data requiring a single function call by the consumer upon implementation.</li>
  </ul>

  <h3>Challenge</h3>
  <p>
    Creating data-driven graphics generally involves the manual coding of data mapped to various serialized components.
    This makes the creation of graphics a very programmer-first approach and is not very agile.
  </p>
  <p>
    This also creates an ever-increasing code base to maintain as the graphics increase in scope and new graphics are created.
  </p>

  <h3>Solution</h3>
  <p>
    Use an MVVM (Model-View-ViewModel) approach with reusable components that define which JSON fields to bind to UGUI elements. These binders connect to a parent Data Binder, which supplies JSON data from the graphic class. The graphic class interacts only with the parent binder, keeping binding logic abstracted and enabling rapid, code-free modifications once implemented.
  </p>

  <h3>Impact</h3>
  <p>
    Vastly improved turn-around speed and build stability for graphic modifications, additions, and changes to data binding.
  </p>
  <ul>
    <li><strong>Before ➙</strong> days of development time with extra QA required to catch bugs and issues</li>
    <li><strong>After ➙</strong> hours of integration time with next to zero QA required since design and data modifications resulted in zero changes to code</li>
  </ul>

  <h3>Key Learnings</h3>
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
  <summary><strong>"Image Blend Component & Shader"</strong><br>A custom Unity Image component and HLSL shader that brings standard creative-tool blend modes into UGUI, allowing designers to implement complex, dynamic image blending without visual compromises.</summary>
  
  <blockquote>
  <h2>Responsibilities</h2>
  <ul>
    <li>Extended the Unity Image component to support a custom shader that allows for various standard image blend types found across all creative tools (darken, multiply, color burn, etc).</li>
    <li>Created a custom HLSL UI shader that supports the various image blend modes and maintains all basic image material functionality.</li>
  </ul>

  <h2>Challenge</h2>
  <p>
    Reproducing designs from tools like Figma or Photoshop can be limited by Unity’s lack of standard blend modes. While final blended images can sometimes be imported directly, dynamic layering often requires in‑engine work, leading to discrepancies between the original design and Unity’s output.
  </p>

  <h2>Solution</h2>
  <p>
    Extend the already provided Unity UGUI Image component to offer one that supports the various blend modes from standard creative software. By creating a shader that handles all of the blend modes math and defaulting the extended Image component with that shader, we enable the user to follow the same pipeline as the designer does, reducing implementation frictions and increases design fidelity.
  </p>

  <h2>Impact</h2>
  <p>
    Vastly improved the design fidelity for designs needing dynamicly blended images. 
  </p>
  <ul>
    <li><strong>Before ➙</strong> design compromises were not uncommon when faced with dynamic blending scenarios, which reduced the designers creative freedom.</li>
    <li><strong>After ➙</strong> designers no longer needed to compromise their vision when working with dynamically blended content</li>
  </ul>

  <h2>Key Learnings</h2>
  <ul>
    <li>Removing creative friction through targeted technical solutions improves both product quality and cross-disciplinary collaboration.
    </li>
    <li>
      Aligning engine capabilities with creative tools strengthens trust between designers and engineers.
    </li>
    <li>
      <strong>Happy designer = happy developer.</strong>
    </li>
  </ul>

  </blockquote>
</details>

<details>
  <summary><strong>"Event-Driven Contrast Ratio Calculator Component"</strong><br>An event-driven Unity component that automatically selects the most readable text color in dynamic backgrounds by calculating contrast ratios at runtime.</summary>
  
  <blockquote>
  <h2>Responsibilities</h2>
  <ul>
    <li>
      Designed and developed a clean, easy to implement component that could calculate the contrast ratio between the color of a background component and 2 text colors and pick the one that was most visible.
    </li>
    <li>
      Designed the component to integrate non-invasively, requiring minimal changes to existing codebases.
    </li>
  </ul>

  <h2>Challenge</h2>
  <p>
    Accessibility is a very important aspect of strong UX and text readability is a top concern in this. When in a scenario where text background is dynamic in nature, it can be very difficult to successfully  curate or maintain a mapping of text colors to background colors. If the scenario is dynamic enough that any color can be present, this becomes near impossible and many edge cases can lead to less accessible UI, while also leading to much larger code bases to handle all of these scenarios. 
  </p>

  <h2>Solution</h2>
  <p>
    Create a component that can calculate the contrast ratio calculate contrast ratios in line with accessibility guidelines (WCAG) between the text background and 2 possible text colors (light and dark variants) and select the color that results in the highest contrast ratio. Since this component needs to be added to an existing code base, its implementation needs to be as non-invasive as possible so an event-driven approach is taken to reduce implementation overhead and offer implementation flexibility.
  </p>

  <h2>Impact</h2>
  <p>
    Resulted in more consistently accessible text in dynamic color scenarios, improving user experience as a whole.
  </p>
  <ul>
    <li><strong>Before ➙</strong> certain dynamic color combinations would be unaccounted for during design which led to reduced accessibility and readability</li>
    <li><strong>After ➙</strong> designers no longer needed to manually map text/background color combinations, reducing design time and the risk of missed accessibility issues</li>
  </ul>

  <h2>Key Learnings</h2>
  <ul>
    <li>
    By automating the choice of text color to be used in a dynamic color scenario to ensure accessibility always met or surpassed web content accessibility guidelines, the resulting UI was more consistently readable across various user bases (across the color blindness spectrum). 
    </li>
    <li>
      This also reduced the load on the designers who no longer needed to worry about creating various color combinations for each foreseeable situation.
    </li>
  </ul>

  </blockquote>
</details>

---

## See for Yourself
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

# Visual Examples
### Data Binder Sample In-Action

---

### Blending Images 

---

### Consistent Readable Text Regardless of Background Color


