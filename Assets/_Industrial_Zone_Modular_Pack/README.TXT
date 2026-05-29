
// Industrial Zone Modular Pack
// For support, feedback and questions : vencious.games@gmail.com

// ==================================================
// -- READ 1. BELOW FOR CORRECT PIPELINE
// ==================================================

1.	How to setup for different pipelines

	NOTE :	at first when you have the package from the package manager imported,
			you will only find the "Pipelines", "Terrains" and "Textures" folder available.
			So in order to get the package working in any pipeline make sure to do the followng:
	
	*	Built-In - navigate to folder "Industrial_Zone_Modular_Pack/Pipelines" and double click and import the "Industral_Zone_BiRP" package.
		You have to import the Post-Processing package from the package manager and the "Industral_Zone_BiRP_Post_Process" package
		from the "Industrial_Zone_Modular_Pack/Pipelines" folder, in order to get the post-processing working in the project and the demo scene.
	*	URP - navigate to folder "Industrial_Zone_Modular_Pack/Pipelines" and double click and import the "Industral_Zone_URP" package.
	*	HDRP - navigate to folder "Industrial_Zone_Modular_Pack/Pipelines" and double click and import the "Industral_Zone_HDRP" package.
	
		NOTE : for the HDRP pipeline, after installing the pipeline pack from the Pipelines folder,
		open the demo scene and select FX/Post-process Volume objcet in the scene
		and assign the volume profile "Industrial_Zone_PP_HDRP" that comes with the pack.
		You can find the asset inside "Industrial_Zone_Modular_Pack/Misc/Post_Processing_Profiles" folder.
		Also the volumetric clouds might not be enabled.
		To enable them, select the "Industrial_Zone_PP_HDRP" asset, opent the "Volumetric Coulds" section
		and check if there's a warning that tells you how to enable the Volumetric Clouds support
		( there must an "Open" button, which will navigate you to the setting you need to enable to get the clouds working )
	
2.	NOTES

	1. Ground Mud texture is terrain only as it contains smoothness in alpha and does not look good on a mesh tile

	2. Post processing stack v3.0+ needs to be added to the project in order to use the post processing profiles indluded in the pack for the build in pipeline

	3. All sets are modular and you create different structures from each one.

	4. The "Industrial_Zone_Factory_Set" is designed as a structure only and player can not get inside


A review and rate are greatly appreciated !

Thank You !