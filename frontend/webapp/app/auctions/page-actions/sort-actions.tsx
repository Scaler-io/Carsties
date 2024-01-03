import { orderButtons } from "@/app/models/filter-options";
import { useParamsStore } from "@/hooks/useParamsStore";
import { Button } from "flowbite-react";
import React from "react";

const SortActions = () => {
  const setParams = useParamsStore((state) => state.setParams);
  const orderBy = useParamsStore((state) => state.orderBy);
  
  return (
    <div>
      <span className="uppercase text-sm text-gray-200 mr-2">Sort</span>
      <Button.Group>
        {orderButtons.map(({ label, icon: Icon , value }) => (
          <Button
            key={value}
            onClick={() => setParams({ orderBy: value })}
            color={`${orderBy === value ? "info" : "gray"}`}
            className="focus:ring-0">
            <Icon className="mr-3 h-4" />
            {label}
          </Button>
        ))}
      </Button.Group>
    </div>
  );
};

export default SortActions;
