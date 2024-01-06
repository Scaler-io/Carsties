import { Label, TextInput } from "flowbite-react";
import React from "react";
import { UseControllerProps, useController } from "react-hook-form";

interface Props extends UseControllerProps {
  label: string;
  type?: string;
  showLabel?: boolean;
}

const TextField = (props: Props) => {
  const { fieldState, field } = useController({ ...props, defaultValue: "" });
  const TextInputStyle = {
    outline: "none",
  };
  return (
    <div className="mb-3">
      {props.showLabel && (
        <div className="mb-2 block">
          <Label htmlFor={field.name} value={props.label} />
        </div>
      )}
      <TextInput
        {...props}
        {...field}
        type={props.type || 'text'}
        placeholder={!props.showLabel && props.label}
        color={fieldState.error ? 'failure' : !fieldState.isDirty ? '' : 'success' }
        style={TextInputStyle}
        autoComplete="off"
        helperText={fieldState.error?.message}
      />
    </div>
  );
};

export default TextField;
