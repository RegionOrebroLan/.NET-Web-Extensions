class RecaptchaOptions {
	public readonly siteKey: string;
	public readonly tokenParameterName: string;

	constructor(siteKey: string, tokenParameterName: string) {
		this.siteKey = siteKey;
		this.tokenParameterName = tokenParameterName;
	}
}

function logDebugIfEnabled(message: string) {
	console.log(`Debug: '${message}'`);
}

function disableFormElements(forms: HTMLFormElement[]): void {
	const disabled = "disabled";
	forms = forms ?? new Array<HTMLFormElement>();

	forms.forEach((form) => {
		form.querySelectorAll("button, fieldset, input, textarea").forEach((element) => {
			element.removeAttribute(disabled);
			element.setAttribute(disabled, disabled);
		});
	});
}

function getRecaptchaForms(): HTMLFormElement[] {
	const recaptchaForms = new Array<HTMLFormElement>();

	document.querySelectorAll("form").forEach((form) => {
		if ((form.getAttribute("data-recaptcha-enabled") ?? "").toLowerCase() === "true") {
			recaptchaForms.push(form);
		}
	});

	return recaptchaForms;
}

function getRecaptchaOptions(): RecaptchaOptions {
	const recaptchaScriptId = "recaptcha-script";
	const recaptchaScript = document.querySelector(`script[id='${recaptchaScriptId}']`);
	if (!recaptchaScript) {
		logDebugIfEnabled(`There is no script-tag with id '${recaptchaScriptId}'.`);
		return null;
	}

	let valid = true;

	const siteKeyAttributeName = "data-recaptcha-siteKey";
	const siteKey = recaptchaScript.getAttribute(siteKeyAttributeName);
	if (!siteKey) {
		valid = false;
		console.error(`The script-tag with id '${recaptchaScriptId}' lacks the attribute '${siteKeyAttributeName}'.`);
	}

	const tokenParameterNameAttributeName = "data-recaptcha-tokenParameterName";
	const tokenParameterName = recaptchaScript.getAttribute(tokenParameterNameAttributeName);
	if (!tokenParameterName) {
		valid = false;
		console.error(`The script-tag with id '${recaptchaScriptId}' lacks the attribute '${tokenParameterNameAttributeName}'.`);
	}

	if (!valid) {
		const recaptchaConfigurationErrorMessageAttributeName = "data-recaptcha-configuration-error-message";
		let recaptchaConfigurationErrorMessage = recaptchaScript.getAttribute(recaptchaConfigurationErrorMessageAttributeName);
		if (!recaptchaConfigurationErrorMessage) {
			recaptchaConfigurationErrorMessage = "reCAPTCHA configuration-error!";
		}

		throw recaptchaConfigurationErrorMessage;
	}

	return new RecaptchaOptions(siteKey, tokenParameterName);
}

function insertRecaptchaInformation(form: HTMLFormElement, recaptchaBadge: HTMLElement) {
	const recaptchaInformation = document.createElement("div");
	recaptchaInformation.className = "mb-3";
	recaptchaInformation.innerHTML = recaptchaBadge.firstElementChild.outerHTML;

	const elementAfterRecaptchaInformation = form.querySelector("[data-element-after-recaptcha-information]") ?? form.querySelector("button[type='submit']");
	elementAfterRecaptchaInformation.parentNode.insertBefore(recaptchaInformation, elementAfterRecaptchaInformation);
}

document.addEventListener("DOMContentLoaded", () => {
	grecaptcha.ready(() => {
		const recaptchaBadge = document.querySelector<HTMLElement>(".grecaptcha-badge");

		if (recaptchaBadge)
			recaptchaBadge.style.visibility = "hidden";

		const recaptchaForms = getRecaptchaForms();

		try {
			const recaptchaOptions = getRecaptchaOptions();

			recaptchaForms.forEach((form) => {
				insertRecaptchaInformation(form, recaptchaBadge);

				form.addEventListener("submit", (e) => {
					e.preventDefault();

					const anchorCharacter = "#";
					let action = form.getAttribute("action");
					logDebugIfEnabled(`Recaptcha-form-action: ${action}`);
					const actionParts = action.split(anchorCharacter);

					if (actionParts.length > 1) {
						actionParts.pop();
						action = actionParts.join(anchorCharacter);
						logDebugIfEnabled(`Recaptcha-form-action without anchor: ${action}`);
					}

					const recaptchaAction = action.replace(/[^a-zA-Z_]/g, "_").substring(0, 100); // Only A-Z, a-z and _ are supported and a maximum length of 100.
					logDebugIfEnabled(`Recaptcha-action: ${recaptchaAction}`);

					grecaptcha.execute(recaptchaOptions.siteKey, { action: recaptchaAction }).then(token => {
						form.insertAdjacentHTML("afterbegin", `<input name="${recaptchaOptions.tokenParameterName}" type="hidden" value="${token}" />`);
						form.submit();
					});
				});
			});
		}
		catch (exception) {
			disableFormElements(recaptchaForms);
			alert(exception);
		}
	});
});