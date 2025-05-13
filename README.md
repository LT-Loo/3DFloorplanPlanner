# 3D Floor Planner
This is a simple 3D retail floor planning tool made in Unity. It allows users plan and design store layouts using common store components such as shelf, fridge and checkout counter.
<br><br>
Try it now on [Itch.io](https://lt-loo.itch.io/3d-foorplan-planner)!

## Code Structure
```
|--- Assets
|      | ---- Images  ## Store tool icons and prefab images
|      |        | ----- Camera Tools
|      |        | ----- Components
|      |
|      | ---- Materials
|      | ---- Prefabs  ## Component prefabs
|      | ---- Scenes
|      | ---- Scripts
|               | ----- Camera  ## Manage camera view and toolbar
|               |         | ---- CameraSystem.cs
|               |         | ---- CameraToggle.cs
|               |
|               | ----- Component  ## Manage component functions, panel, properties etc.
|               |         | ----- Manager.cs
|               |         | ----- ComponentMenu.cs
|               |         | ----- ...
|               |
|               | ----- BorderCollisions.cs  ## To keep components within floorplan
.
.
.
```

## How To Use
### 1. View Controls
Floorplan view can be controlled using the <b>Camera Toolbar</b> on the upper left corner or via shortkeys.
| Tool | Function | Shortkeys |
| :---: | --- | --- |
| Pan Toggle (Hand Icon) | Pan view - Toggle button, click anywere on the scene and drag cursor to move view | Ctrl + Left Mouse Key |
| Zoom In | Zoom into view - Click once to zoom in | Mouse scroll wheel or two fingers sliding up and down on touchpad |
| Zoom Out | Zoom out of view - Click once to zoom out | Mouse scroll wheel or two fingers sliding up and down on touchpad |
| Rotate Toggle | Rotate view - Toggle button, click anywhere on the scene and drag cursor to rotate view | Ctrl + Left Mouse Key |
| Top Down Toggle | Show floorplan from above | N/A |

### 2. Component Placement
#### Component Menu
The component menu is placed at the bottom of the screen. It contains some basic components which can be added onto the floorplan.

#### Add New Component
Click on one of the components. The toggle background will turn green, meaning that the component has been selected. Move your cursor onto the floorplan and you will see a new component slides towards your cursor. <br>
> <i>Only one component can be selected at a time. The component menu will be disabled when any of the components on the floorplan is being moved.</i>

#### Move Component
When a component on the floorplan appears green, it means that it is movable and rotatable. A new component always appears green when created. To move an existing component, click on the component and it is ready to be moved when turned green.

#### Place Component
To place/drop a component, simply click the green component to lock its position. It will then resume to its normal state. <br>
> <i>Note that components can only be moved or placed within the floorplan.</i>

#### Remove Component
To remove a component from the floorplan, simply press the delete key on your keyboard or via the Delete button on the component panel.

#### Component Panel
Whenever a component is created or selected, a component panel will appear on the right side. The component panel shows the name, position and rotation details of the selected component. You can edit the name and values of the component. You can also use the scrollbar to rotate your component. To access the panel of a component, you must double click the component (select and drop) to retrieve its data.

## Original Feature - Component Overlap Detection
When a component is movable, it turns red when overlapping with other components, indicating that it can't be place on that location. It can't be released or dropped and stay movable until it stops overlapping with other components.

### Purpose
- <b>Prevent layout errors</b> to avoid physically impossible and inefficient layouts, helping users understand layout issues without 3D design skills. 
- <b>Cost and time effective</b> by reducing the need for manual checking or trial-and-error, preventing costly errors during real-world setup.

## Limitations
### Lack of Rotation Interaction
Rotation of component can only be applied through the input field and scrollbar and not by direcly manipulating the object, resulting in inefficiency and slower workflow.

### Non-Resizable Component Constrains
Users are not able to resize components like shelves, fridges and display stands. This limits layout flexibility, making it harder to fit components into custom spaces. This may affect usability for users who need precise or adaotable designs.

## Future Ideas
### Customisable Floorplan Layout
Offeing customisable floorplan size and shape would give users greater flexibility to design their layouts. This enables more accurate planning, especially for irregular spaces. This helps improve realism and support a wide range of use cases for different types of stores.

### Sales-Driven Layout Optimisation
Utilising sales analysis to inform stock placement and display strategies can help identify high-performing products and optimise floor space. This can impove overall sales and efficiency by strategically displaying popular items in high-traffic areas.

## Developer
Ler Theng Loo<br>
[loo.workspace@gmail.com](loo.workspace@gmail.com)

  
