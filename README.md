# Bootcamp Brawlers Code Samples

Welcome to the "Bootcamp Brawlers Code Samples" repository! This is a public space where you can explore code snippets and samples from the development of Bootcamp Brawlers, a local party multiplayer game.

## About Bootcamp Brawlers

Bootcamp Brawlers is an exciting local multiplayer game designed for parties and gatherings. It offers a unique gaming experience where players engage in thrilling brawls, bringing friends together for endless fun.

## Code Samples Overview

This repository provides a glimpse into the codebase of Bootcamp Brawlers. Explore the different aspects of game development, from player mechanics to interactive environments. The code samples showcase the implementation of key features, offering insights into the game's development process.

## Getting Started

Feel free to browse through the code samples to gain an understanding of how various components are structured and implemented in Bootcamp Brawlers. If you have any questions or feedback, don't hesitate to reach out.

## Code Highlights

### [Sample 1: Main Menu Manager](#)
Explore the heart of Bootcamp Brawlers' user interface with the MainMenuManager code. This essential component is dedicated to creating an immersive and customized navigation experience within the game's main menu. Delve into the code to understand how UI elements are orchestrated, enabling seamless interaction and a user-friendly journey through the Bootcamp Brawlers experience.


```c#
public void OnNavigate(InputAction.CallbackContext context)
    {
        if (!canNavigate && !transitionEnded)
            return;
        Vector2 inputDirection = context.ReadValue<Vector2>();
        Selectable currentSelectable = selectedItem.GetComponent<Selectable>();
        Selectable nextSelectable = null;
 
        if (inputDirection.y > 0)
        {
            nextSelectable = currentSelectable.FindSelectableOnUp();
        }
        else if (inputDirection.y < 0)
        {
            nextSelectable = currentSelectable.FindSelectableOnDown();
        }
        else if (inputDirection.x > 0)
        {
            nextSelectable = currentSelectable.FindSelectableOnRight();
        }
        else if (inputDirection.x < 0)
        {
            nextSelectable = currentSelectable.FindSelectableOnLeft();
        }

        if (nextSelectable != null)
        {
            //TODO Aqui ocurre la navegacion entre botones
            AudioManager.Instance.PlaySfxPlayer(AudioManager.Instance.moveUI); //MOVE UI
            ChangeColorToNormal(currentSelectable.gameObject);
            currentSelectable = nextSelectable;
            selectedItem = nextSelectable.gameObject;
            ChangeColorToSelected(selectedItem);

            StartCoroutine(NavigationDelay());
        }

        if (currentSelectable is Slider slider)
        {
            float newValue = slider.value;
            if (inputDirection.x > 0)
                newValue += 1; // adjust the value as needed
            else if (inputDirection.x < 0)
                newValue -= 1; // adjust the value as needed
            slider.value = Mathf.Clamp(newValue, slider.minValue, slider.maxValue);
        }

    }
