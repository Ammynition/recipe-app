document.addEventListener("DOMContentLoaded", async (event) => {
    const urlParameters = new URLSearchParams(window.location.search);
    const id = urlParameters.get("recipe");
    await viewRecipe(id);      
    console.log("Got recipe!");
});
/**
 * 
 * @param {string} id 
 */
async function viewRecipe(id){
    const recipeCard = document.getElementById("recipe-card");
    const httpResponse = await fetch(`http://localhost:8080/get-recipe`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            recipe_id: id
        }),
    });

    const jsonResponse = await httpResponse.json();
    
    let recipe = jsonResponse.recipe; 
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
    recipeCard.appendChild(div);
}