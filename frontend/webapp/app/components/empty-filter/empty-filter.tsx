import { useParamsStore } from "@/hooks/useParamsStore";
import React from "react";
import Heading from "../heading/heading";
import { Button } from "flowbite-react";

interface Props {
  title?: string;
  subtitle?: string;
  showReset?: boolean;
}

const EmptyFilter = ({
  title = "No matches for this filter",
  subtitle = "Try chanhing or restting the filer",
  showReset,
}: Props) => {
  const reset = useParamsStore((state) => state.reset);

  return (
    <div className="h-[40vh] flex flex-col gap-2 justify-center items-center shadow-lg">
      <Heading title={title} subtitle={subtitle} center />
      <div className="mt-4">
        {showReset && (
          <Button outline onClick={reset} color="info">
            Remove filters
          </Button>
        )}
      </div>
    </div>
  );
};

export default EmptyFilter;
