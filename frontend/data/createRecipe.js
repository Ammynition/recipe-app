document.addEventListener("DOMContentLoaded", (event) => {
    for (const button of document.getElementsByClassName("create-button"))
    {
        button.addEventListener("click", async (event) => {
            const recipeName = document.getElementsByClassName("recipe-name")[0].value;
            const author = document.getElementsByClassName("author")[0].value;
            const tags = document.getElementsByClassName("tags")[0].value;

            const response = await fetch(`http://localhost:8080/recipe`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    name: recipeName,
                    author: author,
                    tags: tags.split(","),
                    /*ingredients: [],
                    body: "",
                    */
                    
                })
            });
        });
    } 
});

//TODO: Add tags e.g. vegetarian/indian/etc 