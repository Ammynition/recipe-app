document.addEventListener("DOMContentLoaded", async (event) => {

    document.querySelector("button.search").addEventListener("click", async (event) =>{
        await listRecipes();
    });
        
    console.log("Got recipes!");
});

async function listRecipes(){
    const recipesList = document.getElementById("recipes-list");
    //emptying element so page is clear when new search happens
    recipesList.innerHTML = "";
    const httpResponse = await fetch(`http://localhost:8080/recipes`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            query: document.querySelector("#searchRecipes").value,
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
        `;
        div.addEventListener("click", async (event) =>{
            window.location.href = "/viewRecipe.html?recipe=" + recipe.id;
        });
        recipesList.appendChild(div);
    }
}