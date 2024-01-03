import { pageSizeOptions } from "@/app/models/filter-options";
import { useParamsStore } from "@/hooks/useParamsStore";
import { Button } from "flowbite-react";
import React from "react";

const PagingActions = () => {
  const setParams = useParamsStore((state) => state.setParams);
  const pageSize = useParamsStore((state) => state.pageSize);
  return (
    <div>
      <span className="uppercase text-sm text-gray-500 mr-2">Page size</span>
      <Button.Group>
        {pageSizeOptions.map((value, i) => (
          <Button
            key={i}
            onClick={() => {
              setParams({ pageSize: value });
            }}
            color={`${pageSize === value ? "info" : "gray"}`}
            className="focus:ring-0">
            {value}
          </Button>
        ))}
      </Button.Group>
    </div>
  );
};

export default PagingActions;
