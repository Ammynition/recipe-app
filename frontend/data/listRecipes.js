document.addEventListener("DOMContentLoaded", async (event) => {

    const recipesList = document.getElementById("recipes-list");

    const httpResponse = await fetch(`http://localhost:8080/recipes`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            name: "name",
            author: "author",
            tags: ["a"],
            ingredients: [{quantity: 1, unit: "a", name: "b"}],
            body: "a",
        }),
    });

    const jsonResponse = await httpResponse.json();
    
    for (let recipe of jsonResponse.recipes) {
        const ingredients = [];
        if (recipe.Ingredients) {
            for (let ingredient of recipe.Ingredients) {
                ingredients.push(`
                    <div>
                        ${ingredient.quantity} 
                        ${ingredient.unit}
                        ${ingredient.name}
                    </div>
                `);
            }
        }
        const div = document.createElement("div");
        div.className = "recipe";
        div.innerHTML = `
            <div class="attribute name">
                <div class="label">Name</div>
                <div class="value">${recipe.Name}</div>
            </div>
            <div class="attribute author">
                <div class="label">Author</div>
                <div class="value">${recipe.Author}</div>
            </div>
            <div class="attribute tags">
                <div class="label">Tags</div>
                <div class="value">${recipe.Tags}</div>
            </div>
            <div class="attribute ingredients">
                <div class="label">Ingredients</div>
                ${ingredients.join("")}
            </div>
            <div class="attribute body">
                <div class="label">Instructions</div>
                <div class="value"><p>${recipe.Body.split("\n").join("</p><p>")}</p></div>
            </div>
        `;
        recipesList.appendChild(div);
    }

    console.log("Got recipes!");
});

//TODO: add bar to both pages CREATE and RECIPES
//TODO: add a search bar to filter out recipes
//make it a fuzzy search 
//e.g. "Ammy" for author but full name is "Ammy M.", still return Ammy